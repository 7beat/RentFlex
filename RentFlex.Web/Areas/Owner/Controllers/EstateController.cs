using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentFlex.Application.Features.Estates.Queries;
using System.Security.Claims;

namespace RentFlex.Web.Areas.Owner.Controllers;
[Area("Owner")]
public class EstateController : Controller
{
    private readonly IMediator _mediator;
    public EstateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var estates = await _mediator.Send(new GetAllEstatesQuery(Guid.Parse(userId!)));

        return View(estates);
    }
}
