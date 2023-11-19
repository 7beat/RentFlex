using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using RentFlex.Domain.entities;

namespace RentFlex.Infrastructure.Data;
internal class EstatesConfiguration : IEntityTypeConfiguration<Estate>
{
    public void Configure(EntityTypeBuilder<Estate> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.EstateType).HasConversion<string>();
        builder.HasMany(e => e.Rentals).WithOne(e => e.Estate).IsRequired(false);
        builder.HasOne(e => e.ApplicationUser).WithMany(u => u.Estates).HasForeignKey(e => e.UserId);
        builder.Property(e => e.ImageUrls)
            .HasConversion(
            x => JsonConvert.SerializeObject(x),
            x => JsonConvert.DeserializeObject<List<string>>(x)!)
            .IsRequired(false);

        builder.HasData(new Estate()
        {
            Id = Guid.Parse("555daf1f-c760-48d4-9fcf-410cec349f23"),
            PropertyName = "TestProperty",
            IsAvailable = true,
            CostPerDay = 200,
            EstateType = EstateType.Apartment,
            //OwnerId = "6aa43469-b1c8-42b1-aa67-b7240a575f0a",
            // For simplicity Preview model has same references for both services
            BookingReference = Guid.Parse("9d1063e1-125e-45c6-bef3-d5baaa717152"),
            AirbnbReference = Guid.Parse("9d1063e1-125e-45c6-bef3-d5baaa717152")
        });

        builder.OwnsOne(e => e.Address).HasData(new
        {
            EstateId = Guid.Parse("555daf1f-c760-48d4-9fcf-410cec349f23"),
            Country = "Poland",
            City = "Gdańsk",
            PostalCode = "80-342",
            StreetName = "Grunwaldzka",
            PropertyNumber = 11
        });
    }
}

internal class RentalsConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RentType)
            .HasConversion<string>();

        builder.HasData(new Rental()
        {
            Id = Guid.NewGuid(),
            EstateId = Guid.Parse("555daf1f-c760-48d4-9fcf-410cec349f23"),
            RentType = RentType.ShortTerm,
            StartDate = DateOnly.FromDateTime(DateTime.Today),
            EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
        });
    }
}
