using RentFlex.Domain.entities;

namespace RentFlex.Application.Models;
public record EstateDto
{
    public Guid Id { get; set; }
    public string PropertyName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsAvailable { get; set; }
    public double CostPerDay { get; set; }
    public EstateType EstateType { get; set; }
    public string ThumbnailImageUrl { get; set; } = default!;
    public List<string> ImageUrls { get; set; } = default!;
    public Guid OwnerId { get; set; }

    // Address
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string StreetName { get; set; } = default!;
    public int PropertyNumber { get; set; }

    public bool PublishedAirbnb { get; set; }
}
