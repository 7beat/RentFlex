namespace RentFlex.Application.Contracts.Infrastructure.Services;
public interface IStorageService
{
    Task<string> AddAsync(Stream stream, string fileExtension, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string blobUrl, CancellationToken cancellationToken = default);
    Task PersistDbAsync(Stream stream, CancellationToken cancellationToken = default);
}
