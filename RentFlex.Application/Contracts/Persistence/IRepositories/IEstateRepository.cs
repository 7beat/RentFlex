using RentFlex.Domain.entities;

namespace RentFlex.Application.Contracts.Persistence.IRepositories;
public interface IEstateRepository
{
    void Update(Estate estate);
}
