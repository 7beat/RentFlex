using RentFlex.Domain.common;
using RentFlex.Domain.Entities;

namespace RentFlex.Domain.entities;
public class Estate : EntityBase
{
    public string PropertyName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsAvailable { get; set; }
    public double CostPerDay { get; set; }
    public EstateType EstateType { get; set; }
    public string? ThumbnailImageUrl { get; set; } = default!;
    public List<string> ImageUrls { get; set; } = default!;
    public Address Address { get; set; } = default!;

    public Guid? BookingReference { get; set; }
    public Guid? AirbnbReference { get; set; }

    public ICollection<Rental> Rentals { get; set; } = default!;
    public Guid? UserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; } = default!;
}

// Owned
public class Address
{
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string StreetName { get; set; } = default!;
    public int PropertyNumber { get; set; }
}

public enum EstateType
{
    House,
    Apartment,
    Villa
}
