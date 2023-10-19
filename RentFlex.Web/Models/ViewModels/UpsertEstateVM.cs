using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentFlex.Application.Features.Estates.Commands;
using RentFlex.Domain.entities;
using RentFlex.Domain.Enums;

namespace RentFlex.Web.Models.ViewModels;

public class UpsertEstateVM
{
    public UpsertEstateCommand Estate { get; set; } = default!;
    public int ThumbnailImage { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> EstateType { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> Country { get; set; }

    public UpsertEstateVM()
    {
        EstateType = Enum.GetValues(typeof(EstateType))
            .Cast<EstateType>()
            .Select(type => new SelectListItem
            {
                Value = type.ToString(),
                Text = type.ToString()
            });

        Country = Enum.GetValues(typeof(EuropeanCountry))
            .Cast<EuropeanCountry>()
            .Select(type => new SelectListItem
            {
                Value = type.ToString(),
                Text = type.ToString()
            });
    }
}
