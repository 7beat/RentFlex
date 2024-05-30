using Azure.Storage.Blobs;
using RentFlex.Application.Contracts.Infrastructure.Services;

namespace RentFlex.Infrastructure.Services;
public class StorageService(BlobServiceClient blobServiceClient) : IStorageService
{
    public async Task<string> AddAsync(Stream stream, CancellationToken cancellationToken)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient("images");
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString());

        var response = await blobClient.UploadAsync(stream, cancellationToken);

        return blobClient.Uri.AbsoluteUri;
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

}
