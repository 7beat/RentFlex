﻿using RentFlex.Domain.entities;

namespace RentFlex.Application.Features.Estates;
public record EstateDto
{
    public string PropertyName { get; set; } = default!;
    public bool IsAvailable { get; set; }
    public double CostPerDay { get; set; }
    public EstateType EstateType { get; set; }
    public ICollection<string> ImageUrls { get; set; } = default!;
    public Guid OwnerId { get; set; }

    // Address
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string StreetName { get; set; } = default!;
    public int PropertyNumber { get; set; }
}