using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;
using RentFlex.Domain.entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RentFlex.Application.Features.Estates.Commands;
public record UpsertEstateCommand : IRequest
{
    public Guid? Id { get; set; }
    [DisplayName("Property Name")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "This needs to be minimum {2} and maximum {1} characters long")]
    public string PropertyName { get; set; } = default!;
    [DataType(DataType.MultilineText)]
    [StringLength(500, MinimumLength = 3, ErrorMessage = "This needs to be minimum {2} and maximum {1} characters long")]
    public string Description { get; set; } = default!;
    [DisplayName("Cost Per Day")]
    public double CostPerDay { get; set; }
    [DisplayName("Type of Estate")]
    public EstateType EstateType { get; set; }
    public Guid OwnerId { get; set; }

    // Images
    [ValidateNever]
    public IEnumerable<IFormFile> Images { get; set; } = default!;
    public int ThumbnailImage { get; set; }
    [ValidateNever]
    [DisplayName("Images")]
    public List<string> ImageUrls { get; set; } = default!;

    // Address
    public string? Country { get; set; }
    public string? City { get; set; }
    [DisplayName("Postal Code")]
    public string? PostalCode { get; set; }
    public string? StreetName { get; set; }
    public int? PropertyNumber { get; set; }

    // External
    [DisplayName("Publish to Airbnb")]
    public bool PublishAirbnb { get; set; }
    [DisplayName("Publish to Booking")]
    public bool PublishBooking { get; set; }
}

internal class UpsertEstateCommandHandler : IRequestHandler<UpsertEstateCommand>
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IAirbnbService airbnbService;
    private readonly IStorageService storageService;
    public UpsertEstateCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IAirbnbService airbnbService, IStorageService storageService)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        this.airbnbService = airbnbService;
        this.storageService = storageService;
    }

    public async Task Handle(UpsertEstateCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.FindSingleAsync(u => u.Id == request.OwnerId, cancellationToken) ??
            throw new Exception("User with given Id was not found!");

        if (request.Id is null)
        {
            var estate = mapper.Map<Estate>(request);

            user.Estates ??= new List<Estate>();
            user.Estates.Add(estate);

            if (request.Images is not null)
            {
                var imageUrls = await PersistImagesAsync(request.Images, cancellationToken);
                estate.ImageUrls ??= new(imageUrls);
                estate.ThumbnailImageUrl = imageUrls.First();
            }

            estate.AirbnbReference = !request.PublishAirbnb || user.AirbnbReference is null ?
                null :
                await airbnbService.CreateEstateAsync(user.AirbnbReference!.Value, mapper.Map<EstateDto>(estate));

            // estate.BookingReference = request.PublishBooking is false ? // ToDo: Finish when service is ready
        }
        else
        {
            var estateDb = user.Estates.First(e => e.Id == request.Id);

            mapper.Map(request, estateDb);

            if (request.Images is not null)
            {
                var imageUrls = await PersistImagesAsync(request.Images, cancellationToken);

                var newImages = estateDb.ImageUrls is null ?
                    new List<string>(imageUrls) :
                    estateDb.ImageUrls.Concat(imageUrls);

                estateDb.ImageUrls = new List<string>(newImages);
            }

            if (estateDb!.ImageUrls[request.ThumbnailImage] != estateDb.ThumbnailImageUrl)
            {
                estateDb.ThumbnailImageUrl = estateDb.ImageUrls[request.ThumbnailImage];
            }

            if (request.PublishAirbnb && estateDb.AirbnbReference is null)
            {
                estateDb.AirbnbReference = user.AirbnbReference is null ?
                    null :
                    await airbnbService.CreateEstateAsync(user.AirbnbReference!.Value, mapper.Map<EstateDto>(estateDb));
            }

            unitOfWork.Estates.Update(estateDb);
        }

        await unitOfWork.SaveChangesAsync();
    }

    private async Task<IEnumerable<string>> PersistImagesAsync(IEnumerable<IFormFile> images, CancellationToken cancellationToken)
    {
        var tasks = images.Select(async image =>
        {
            using var stream = image.OpenReadStream();
            var extension = Path.GetExtension(image.FileName);
            return await storageService.AddAsync(stream, extension, cancellationToken);
        });

        var imageUrls = await Task.WhenAll(tasks);
        return imageUrls;
    }
}
