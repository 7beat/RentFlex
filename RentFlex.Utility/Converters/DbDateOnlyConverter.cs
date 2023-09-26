using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RentFlex.Utility.Converters;

/// <summary>
/// Converter that seamlessly transforms <see cref="DateOnly" /> instances to <see cref="DateTime"/> and vice versa.
/// </summary>
public class DbDateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DbDateOnlyConverter() : base(
        d => d.ToDateTime(TimeOnly.MinValue),
        d => DateOnly.FromDateTime(d))
    { }
}
