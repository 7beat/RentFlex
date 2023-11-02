using Microsoft.AspNetCore.Identity;
using RentFlex.Application.Contracts.Identity;
using RentFlex.Infrastructure.Identity;

namespace RentFlex.Infrastructure.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager;

    public AuthService(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<bool> IsUserAuthorized(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return false;

        var role = await userManager.IsInRoleAsync(user, "Admin"); // Add AppRoles

        return role;
    }

    public async Task<string> GetUserRoleAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return string.Empty;

        var roles = await userManager.GetRolesAsync(user);

        var role = roles.FirstOrDefault();
        return role!;
    }
}
