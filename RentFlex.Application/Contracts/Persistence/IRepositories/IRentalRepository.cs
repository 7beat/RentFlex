using RentFlex.Domain.entities;

namespace RentFlex.Application.Contracts.Persistence.IRepositories;
public interface IRentalRepository : IGenericRepository<Rental>
{
    void Update(Rental rental);
}
