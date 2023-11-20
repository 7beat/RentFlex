using RentFlex.Domain.entities;

namespace RentFlex.Application.Models;
public class RentalDto
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public RentType RentType { get; set; } // Rental Should have CostPerDay
    public string EstateName { get; set; } = default!;
    //public string EstateDescription { get; set; }
}
