﻿using Microsoft.Graph;
using Microsoft.Graph.Models;
using RentFlex.Application.Contracts.Infrastructure.Services;

namespace RentFlex.Infrastructure.Services;

public class GraphService(GraphServiceClient graphClient) : IGraphService
{
    public async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await graphClient.Users[userId.ToString()]
                .GetAsync(cancellationToken: cancellationToken);

            return user;
        }
        catch (ServiceException ex)
        {
            throw new Exception("Error retrieving user from Graph API", ex);
        }
    }

    public async Task<int> GetUsersCountAsync(CancellationToken cancellationToken)
    {
        var users = await graphClient.Users.GetAsync(cancellationToken: cancellationToken);

        return users?.Value?.Count ?? 0;
    }
}