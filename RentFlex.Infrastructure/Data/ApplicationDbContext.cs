using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentFlex.Domain.entities;
using RentFlex.Domain.Entities;
using RentFlex.Utility.Converters;
using System.Reflection;

namespace RentFlex.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<Estate> Estates { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DbDateOnlyConverter>()
            .HaveColumnType("date");
    }
}
