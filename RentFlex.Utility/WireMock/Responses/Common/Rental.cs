namespace RentFlex.Utility.WireMock.Responses.Common;
public class Rental
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public RentType RentType { get; set; }
}

public enum RentType
{
    ShortTerm,
    LongTerm
}