﻿using MediatR;
using Microsoft.Graph.Models;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Domain.Entities;

namespace RentFlex.Application.Features.Users.Commands;
public record CreateUserNotification(Guid UserId) : INotification;

public class CreateUserNotificationHandler(IGraphService graphService, IUnitOfWork unitOfWork) : INotificationHandler<CreateUserNotification>
{
    public async Task Handle(CreateUserNotification notification, CancellationToken cancellationToken)
    {
        var user = await graphService.GetUserAsync(notification.UserId, cancellationToken);
        await unitOfWork.Users.AddAsync(MapUser(user!), cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private ApplicationUser MapUser(User user) =>
        new()
        {
            Id = Guid.Parse(user.Id!),
            Username = user.DisplayName!
        };
}
