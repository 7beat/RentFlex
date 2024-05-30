namespace RentFlex.Application.Contracts.Infrastructure.Services;
public interface IStorageService
{
    Task<string> AddAsync(Stream stream, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string blobUrl, CancellationToken cancellationToken = default);
}
