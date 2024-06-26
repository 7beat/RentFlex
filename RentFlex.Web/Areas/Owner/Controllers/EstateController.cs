﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentFlex.Application.Features.Estates.Commands;
using RentFlex.Application.Features.Estates.Queries;
using RentFlex.Web.Models.ViewModels;
using System.Security.Claims;

namespace RentFlex.Web.Areas.Owner.Controllers;
[Authorize]
[Area("Owner")]
public class EstateController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public EstateController(IMediator mediator, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var estates = await _mediator.Send(new GetAllEstatesQuery(Guid.Parse(userId!)));

        return View(estates);
    }

    public async Task<IActionResult> Upsert(Guid? id)
    {
        UpsertEstateVM estateVM = new()
        {
            Estate = new(),
        };

        if (id is not null)
        {
            var estateDto = await _mediator.Send(new GetSingleEstateQuery(id.Value));
            estateVM.Estate = _mapper.Map<UpsertEstateCommand>(estateDto);
        }

        return View(estateVM);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(UpsertEstateVM estateVM, List<IFormFile> estateImages)
    {
        if (ModelState.IsValid)
        {
            estateVM.Estate.OwnerId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (estateImages.Any())
                estateVM.Estate.Images = estateImages;

            await _mediator.Send(estateVM.Estate);
        }

        string action = estateVM.Estate.Id is null ? "created" : "updated";
        TempData["success"] = $"Estate {action} successfully";

        return RedirectToAction(nameof(Index));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteEstateCommand(id));

        TempData[result ? "success" : "error"] = result ? "Estate Deleted Sussefully" : "Failed to Delete Estate";
        return Json(new { success = true });
    }

    [Obsolete]
    private IEnumerable<string> PersistImages(List<IFormFile> images)
    {
        var imagePaths = new List<string>();
        string hostImagesPath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads", "images");

        foreach (var file in images)
        {
            string fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);

            var filePath = Path.Combine(hostImagesPath, $"{fileName}{extension}");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            imagePaths.Add($"/uploads/images/{fileName}{extension}");
        }

        return imagePaths;
    }
}
