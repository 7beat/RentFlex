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

            return blobClient.Uri.AbsoluteUri;
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Error occured while uploading Image to Blob: {ex.Message}");
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

            var blobClient = containerClient.GetBlobClient($"{DateTime.Now.ToString("dd.MM.yyyy-HH.mm")}_dbsnapshot.bacpac");

            await blobClient.UploadAsync(stream, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Error occured while persisting Db data: {ex.Message}");
            throw;
        }
    }
}
