using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentFlex.Web.Models;
using System.Diagnostics;

namespace RentFlex.Web.Areas.User.Controllers;

[Area("Owner")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity!.IsAuthenticated)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var estates = await _mediator.Send(new GetAllEstatesQuery(Guid.Parse(userId!)));
            return RedirectToAction(nameof(Index), "Estate");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
