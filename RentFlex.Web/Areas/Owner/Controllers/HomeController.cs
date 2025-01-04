using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentFlex.Application.Constants;
using RentFlex.Application.Features.Home.Queries;
using RentFlex.Web.Models;

namespace RentFlex.Web.Areas.Owner.Controllers;

[Area(WebConstants.OwnerArea)]
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
            return RedirectToAction(nameof(Index), WebConstants.EstateController);
        }

        var applicationStats = await _mediator.Send(new GetApplicationStatsQuery());

        return View(applicationStats);
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
