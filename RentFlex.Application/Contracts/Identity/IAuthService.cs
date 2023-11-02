namespace RentFlex.Application.Contracts.Identity;
public interface IAuthService
{
    Task<bool> IsUserAuthorized(Guid userId);
    Task<string> GetUserRoleAsync(Guid userId);
}
