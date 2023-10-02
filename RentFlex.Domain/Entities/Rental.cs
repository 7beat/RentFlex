using RentFlex.Domain.common;

namespace RentFlex.Domain.entities;
public class Rental : EntityBase
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public RentType RentType { get; set; }
    public Guid EstateId { get; set; }
    public Estate Estate { get; set; } = default!;
}

public enum RentType
{
    ShortTerm,
    LongTerm
}
