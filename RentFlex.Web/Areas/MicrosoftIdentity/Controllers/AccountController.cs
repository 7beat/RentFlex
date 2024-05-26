using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace RentFlex.Web.Areas.MicrosoftIdentity.Controllers;
[Area("MicrosoftIdentity")]
public class AccountController : Controller
{
    public IActionResult SignIn()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "/" });
    }

    public async Task<IActionResult> SignOut()
    {
        var redirectUri = Url.Action("Index", "Home", new { area = "Owner" }, Request.Scheme);

        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUri
        };

        return SignOut(properties, OpenIdConnectDefaults.AuthenticationScheme, "Cookies");
    }
}
