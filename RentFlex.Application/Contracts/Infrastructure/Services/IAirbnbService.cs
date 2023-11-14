using RentFlex.Domain.entities;

namespace RentFlex.Application.Contracts.Infrastructure.Services;
public interface IAirbnbService
{
    Task Test();
    Task<IEnumerable<Estate>> GetAllEstatesAsync(Guid airbnbReference); // userReference
    Task<Guid> CreateEstateAsync(Estate estate);
    Task<bool> UpdateEstateAsync(Guid airbnbReference, Estate estate);
    Task<bool> DeleteEstateAsync(Guid airbnbReference); // estateReference
}
