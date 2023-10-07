using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentFlex.Application.Features.Estates.Commands;

namespace RentFlex.Web.Models.ViewModels;

public class UpsertEstateVM
{
    public UpsertEstateCommand Estate { get; set; } = default!;
    [ValidateNever]
    public IEnumerable<SelectListItem> EstateType { get; set; } = default!;
}
