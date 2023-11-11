using RentFlex.Utility.WireMock.Responses.Common;

namespace RentFlex.Utility.WireMock.Responses;
public class AirbnbEstateResponse
{
    public Guid Id { get; set; }
    public string PropertyName { get; set; } = default!;
    public bool IsAvailable { get; set; }
    public double CostPerDay { get; set; }
    public EstateTypeResponse EstateType { get; set; }
    public string? ThumbnailImageUrl { get; set; } = default!;
    public List<string> ImageUrls { get; set; } = default!;
    public AddressResponse Address { get; set; } = default!;

    public Guid? AirbnbReference { get; set; }

    public ICollection<RentalResponse> Rentals { get; set; } = default!;
    public Guid OwnerId { get; set; }
}