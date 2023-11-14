namespace RentFlex.Utility.WireMock.Responses.Common;
public class AddressResponse
{
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string StreetName { get; set; } = default!;
    public int PropertyNumber { get; set; }
}
