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
        //builder.OwnsOne(e => e.Address); // check this
        builder.Property(e => e.EstateType).HasConversion<string>();
        builder.HasMany(e => e.Rentals).WithOne(e => e.Estate).IsRequired(false);
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
            OwnerId = Guid.Parse("6aa43469-b1c8-42b1-aa67-b7240a575f0a"),
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
