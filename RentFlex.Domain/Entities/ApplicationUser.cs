using Microsoft.AspNetCore.Identity;
using RentFlex.Domain.entities;

namespace RentFlex.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly Birthday { get; set; }
    public Guid? AirbnbReference { get; set; }
    public Guid? BookingReference { get; set; }
    public ICollection<Estate> Estates { get; set; } = default!;
}
