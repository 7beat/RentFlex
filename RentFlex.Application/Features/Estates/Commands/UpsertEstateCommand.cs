﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Domain.entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RentFlex.Application.Features.Estates.Commands;
public record UpsertEstateCommand : IRequest
{
    public Guid? Id { get; set; }
    [DisplayName("Property Name")]
    [StringLength(10, MinimumLength = 3, ErrorMessage = "This needs to be minimum {2} and maximum {1} characters long")]
    public string PropertyName { get; set; } = default!;
    [DisplayName("Cost Per Day")]
    public double CostPerDay { get; set; }
    [DisplayName("Type of Estate")]
    public EstateType EstateType { get; set; }
    public int ThumbnailImage { get; set; }
    [ValidateNever]
    public List<string> ImageUrls { get; set; } = default!;
    public Guid OwnerId { get; set; }

    // Address
    public string? Country { get; set; }
    public string? City { get; set; }
    [DisplayName("Postal Code")]
    public string? PostalCode { get; set; }
    public string? StreetName { get; set; }
    public int? PropertyNumber { get; set; }
}

internal class UpsertEstateCommandHandler : IRequestHandler<UpsertEstateCommand>
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    public UpsertEstateCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
    }

    public async Task Handle(UpsertEstateCommand request, CancellationToken cancellationToken)
    {
        if (request.Id is null)
        {
            var estate = mapper.Map<Estate>(request);
            estate.ImageUrls = new(request.ImageUrls);
            await unitOfWork.Estates.AddAsync(estate);
        }
        else
        {
            var estateDb = await unitOfWork.Estates.FindSingleAsync(e => e.Id == request.Id, cancellationToken);
            mapper.Map(request, estateDb);
            if (request.ImageUrls is not null)
            {
                var newImages = estateDb.ImageUrls is null ?
                    new List<string>(request.ImageUrls) :
                    estateDb.ImageUrls.Concat(request.ImageUrls);

                estateDb.ImageUrls = new List<string>(newImages);
            }

            if (estateDb.ImageUrls[request.ThumbnailImage] != estateDb.ThumbnailImageUrl)
            {
                estateDb.ThumbnailImageUrl = estateDb.ImageUrls[request.ThumbnailImage];
            }

            unitOfWork.Estates.Update(estateDb);
        }

        await unitOfWork.SaveChangesAsync();
    }
}
