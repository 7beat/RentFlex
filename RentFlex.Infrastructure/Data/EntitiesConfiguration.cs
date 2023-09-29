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
        builder.OwnsOne(e => e.Address);
        builder.Property(e => e.ImageUrls)
            .HasConversion(
            x => JsonConvert.SerializeObject(x),
            x => JsonConvert.DeserializeObject<ICollection<string>>(x)!)
            .IsRequired(false);

    }
}
