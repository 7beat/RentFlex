using RentFlex.Domain.entities;

namespace RentFlex.Application.Contracts.Persistence.IRepositories;
internal interface IEstateRepository
{
    void Update(Estate estate);
}
