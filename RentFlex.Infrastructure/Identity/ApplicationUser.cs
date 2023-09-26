using Microsoft.AspNetCore.Identity;
//using RentFlex.Domain.entities;

namespace RentFlex.Infrastructure.Identity;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly Birthday { get; set; }
    //public ICollection<Estate> Estates { get; set; }
}
