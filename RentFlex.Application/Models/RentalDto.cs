using RentFlex.Domain.entities;

namespace RentFlex.Application.Models;
public class RentalDto
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public RentType RentType { get; set; }
    public string PropertyName { get; set; } = default!;
    public double CostPerDay { get; set; }
}
