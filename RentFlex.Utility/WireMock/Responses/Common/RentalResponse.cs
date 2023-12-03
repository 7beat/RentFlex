namespace RentFlex.Utility.WireMock.Responses.Common;
public class RentalResponse
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public RentType RentType { get; set; }
    public required Guid EstateId { get; set; }
    public string PropertyName { get; set; } = default!;
}

public enum RentType
{
    ShortTerm,
    LongTerm
}