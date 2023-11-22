using RentFlex.Application.Models;
using RentFlex.Domain.entities;

namespace RentFlex.Application.Contracts.Infrastructure.Services;
public interface IAirbnbService
{
    Task Test();
    Task<IEnumerable<Estate>> GetAllEstatesAsync(Guid airbnbReference); // userReference
    Task<Guid> CreateEstateAsync(Guid userReference, EstateDto estate);
    Task<bool> UpdateEstateAsync(Guid airbnbReference, EstateDto estate);
    Task<bool> DeleteEstateAsync(Guid airbnbReference); // estateReference
    Task<IEnumerable<RentalDto>> GetAllRentals(Guid userReference, Guid estateReference);
}
