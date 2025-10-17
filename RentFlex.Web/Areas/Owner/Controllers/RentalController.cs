using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentFlex.Application.Features.Rentals.Queries;
using Rotativa.AspNetCore;

namespace RentFlex.Web.Areas.Owner.Controllers;

[Authorize]
[Area("Owner")]
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

    public async Task<IActionResult> DownloadInvoice(Guid invoiceId)
    {
        var rental = await mediator.Send(new GetSingleRentalQuery(invoiceId));

        return new ViewAsPdf("InvoicePdf", rental)
        {
            FileName = $"Invoice_{rental.Id}.pdf",
            PageSize = Rotativa.AspNetCore.Options.Size.A4,
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
        };
    }
}
