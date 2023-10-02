using RentFlex.Domain.entities;

namespace RentFlex.Application.Contracts.Persistence.IRepositories;
public interface IEstateRepository : IGenericRepository<Estate>
{
    void Update(Estate estate);
    //Task<Estate> FindWithOwnerDetails();
}
