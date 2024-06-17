using Microsoft.Graph.Models;

namespace RentFlex.Application.Contracts.Infrastructure.Services;
public interface IGraphService
{
    Task<int> GetUsersCountAsync(CancellationToken cancellationToken);
    Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken);
}
