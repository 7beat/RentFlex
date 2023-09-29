using RentFlex.Domain.common;

namespace RentFlex.Domain.entities;
public class Estate : EntityBase
{
    public Address Address { get; set; }
    public bool IsAvailable { get; set; }
    public double CostPerDay { get; set; }
    public EstateType EstateType { get; set; }
    public ICollection<string> ImageUrls { get; set; } = default!;

    public Guid? BookingReference { get; set; }
    public Guid? AirbnbReference { get; set; }

    public ICollection<Rental> Rentals { get; set; } = default!;
    public Guid OwnerId { get; set; }
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
