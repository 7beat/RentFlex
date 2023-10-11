using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentFlex.Application.Features.Estates.Commands;
using RentFlex.Domain.entities;

namespace RentFlex.Web.Models.ViewModels;

public class UpsertEstateVM
{
    public UpsertEstateCommand Estate { get; set; } = default!;
    public int ThumbnailImage { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> EstateType { get; set; }

    public UpsertEstateVM()
    {
        EstateType = Enum.GetValues(typeof(EstateType))
            .Cast<EstateType>()
            .Select(type => new SelectListItem
            {
                Value = type.ToString(),
                Text = type.ToString()
            });
    }
}
