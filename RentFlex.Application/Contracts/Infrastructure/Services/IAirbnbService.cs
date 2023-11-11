using RentFlex.Domain.entities;

namespace RentFlex.Application.Contracts.Infrastructure.Services;
public interface IAirbnbService
{
    Task Test();
    Task<IEnumerable<Estate>> GetAllEstatesAsync(Guid airbnbReference);
}
