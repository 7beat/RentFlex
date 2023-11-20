using Microsoft.AspNetCore.Mvc;

namespace RentFlex.Web.Areas.Owner.Controllers;
public class RentalController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
