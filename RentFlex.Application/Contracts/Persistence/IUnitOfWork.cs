﻿using RentFlex.Application.Contracts.Persistence.IRepositories;

namespace RentFlex.Application.Contracts.Persistence;
public interface IUnitOfWork
{
    IEstateRepository Estates { get; }
    IUserRepository Users { get; }
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default!);
}
