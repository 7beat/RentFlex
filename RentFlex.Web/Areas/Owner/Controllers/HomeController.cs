using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentFlex.Application.Features.Estates.Queries;
using RentFlex.Web.Models;
using System.Diagnostics;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _mediator.Send(new GetAllEstatesQuery(Guid.Parse(userId!)));
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
