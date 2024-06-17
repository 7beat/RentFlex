using MediatR;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Domain.Entities;

namespace RentFlex.Application.Features.Users.Queries;

public record GetSingleUserQuery(Guid Id) : IRequest<ApplicationUser>
{
    internal class GetSingleUserQueryHandler : IRequestHandler<GetSingleUserQuery, ApplicationUser>
    {
        private readonly IUnitOfWork repository;
        public GetSingleUserQueryHandler(IUnitOfWork repository)
        {
            this.repository = repository;
        }

        public async Task<ApplicationUser> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
        {
            var user = await repository.Users.FindSingleAsync(u => u.Id == request.Id, cancellationToken) ??
                throw new Exception("User with given Id was not found!");

            return user;
        }
    }
}
