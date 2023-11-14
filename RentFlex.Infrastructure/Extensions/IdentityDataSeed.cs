using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentFlex.Infrastructure.Data;
using RentFlex.Infrastructure.Identity;

namespace CarRental.Infrastructure.Extensions;
public static class IdentityDataSeed
{
    /// <summary>
    /// Seeds the database with initial <see cref="ApplicationUser"/> and <see cref="IdentityRole"/> instances for IdentityUsers.
    /// </summary>
    /// <remarks>
    /// This method creates default accounts for administration and regular users. The default password for all accounts is "Password123!".
    /// </remarks>
    /// <param name="app">An instance of <see cref="IApplicationBuilder"/>.</param>
    public static async Task SeedIdentityAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        using var context = new ApplicationDbContext(
        scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        var adminUser = await EnsureUser(serviceProvider, "Admin", "Password123!");
        await EnsureRole(serviceProvider, adminUser, "Admin");

        var normalUser = await EnsureUser(serviceProvider, "User", "Password123!");
        await EnsureRole(serviceProvider, normalUser, "User");
    }

    private static async Task<string> EnsureUser(
    IServiceProvider serviceProvider,
    string userName, string initPw)
    {
        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        var user = await userManager!.FindByNameAsync(userName);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = userName,
                Email = userName + "@gmail.com",
                EmailConfirmed = true,
                FirstName = "John",
                LastName = "Doe",
                Birthday = new(1999, 06, 08),
                AirbnbReference = userName.Equals("User") ? Guid.Parse("592fdf9f-2395-4a12-8f66-1e8b3b53b6fc") : null,
                BookingReference = userName.Equals("User") ? Guid.Parse("592fdf9f-2395-4a12-8f66-1e8b3b53b6fc") : null
            };

            var result = await userManager.CreateAsync(user, initPw);
        }

        if (user is null)
            throw new Exception("User did not get created. Password policy problem?");

        return user.Id;
    }

    private static async Task<IdentityResult> EnsureRole(
        IServiceProvider serviceProvider, string uid, string role)
    {
        var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

        IdentityResult ir;

        if (!await roleManager!.RoleExistsAsync(role))
        {
            ir = await roleManager.CreateAsync(new(role));
        }

        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        var user = await userManager!.FindByIdAsync(uid.ToString());

        if (user is null)
            throw new Exception("User not existing");

        ir = await userManager.AddToRoleAsync(user, role);

        return ir;
    }

}
