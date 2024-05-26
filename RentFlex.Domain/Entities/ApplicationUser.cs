using RentFlex.Domain.entities;

namespace RentFlex.Domain.Entities;
public class ApplicationUser
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public Guid? AirbnbReference { get; set; }
    public Guid? BookingReference { get; set; }
    public ICollection<Estate> Estates { get; set; } = default!;
}
