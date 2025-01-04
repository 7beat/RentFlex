using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentFlex.Application.Constants;
using RentFlex.Application.Features.Rentals.Queries;

namespace RentFlex.Web.Areas.Owner.Controllers;

[Authorize]
[Area(WebConstants.OwnerArea)]
public class RentalController : Controller
{
    private readonly IMediator mediator;
    public RentalController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var rentals = await mediator.Send(new GetAllRentalsQuery(userId!));
        return View(rentals);
    }
}
