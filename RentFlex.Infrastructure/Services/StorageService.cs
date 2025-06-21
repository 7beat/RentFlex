using System.Net;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using RentFlex.Application.Contracts.Infrastructure.Services;

namespace RentFlex.Infrastructure.Services;
public class StorageService(BlobServiceClient blobServiceClient, ILogger<StorageService> logger) : IStorageService
{
    public async Task<string> AddAsync(Stream stream, string fileExtension, CancellationToken cancellationToken)
    {
        try
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("images");
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var blobName = Guid.NewGuid().ToString() + fileExtension;
            var blobClient = containerClient.GetBlobClient(blobName);

            var response = await blobClient.UploadAsync(stream, cancellationToken);

            if (response.GetRawResponse().Status == (int)HttpStatusCode.Created)
                logger.LogInformation("Image succesfully uploaded to Blob Storage with url: {absoluteUri}", blobClient.Uri.AbsoluteUri);

            var envFriendlyPath = MakeEnvironmentFriendlyPath(blobClient.Uri.AbsoluteUri);

            return envFriendlyPath;
        }
        catch (Exception ex)
        {
            logger.LogWarning("Error occured while uploading Image to Blob Storage: {message}", ex.Message);
            throw;
        }

    }

    public async Task<bool> DeleteAsync(string blobUrl, CancellationToken cancellationToken)
    {
        var uri = new Uri(blobUrl);
        var blobName = uri.Segments.Last();

        var containerClient = blobServiceClient.GetBlobContainerClient("images");
        var blobClient = containerClient.GetBlobClient(blobName);

        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        return response.Value;
    }

    public async Task PersistDbAsync(Stream stream, CancellationToken cancellationToken)
    {
        try
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("db-snapshots");
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var blobClient = containerClient.GetBlobClient($"{DateTime.Now:dd.MM.yyyy-HH.mm}_dbsnapshot.bacpac");

            await blobClient.UploadAsync(stream, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning("Error occured while persisting Db data: {message}", ex.Message);
            throw;
        }
    }

    private static string MakeEnvironmentFriendlyPath(string absoluteUri)
    {
        const string azuriteHost = "rentflex-azurite:10000";

        if (!absoluteUri.Contains(azuriteHost))
            return absoluteUri;

        string replacementHost = IsRunningInKubernetes() ? "127.0.0.1:30000" : "127.0.0.1:10000";

        var test = absoluteUri.Replace(azuriteHost, replacementHost);
        return test;
    }

    private static bool IsRunningInKubernetes() =>
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST"));
}
