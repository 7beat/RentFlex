using RentFlex.Utility.WireMock.Responses;
using RentFlex.Utility.WireMock.Responses.Common;

namespace RentFlex.Utility.WireMock;
internal static class ResponseGenerator
{
    public static AirbnbEstateResponse GetSingleAirbnbEstateResponse() => // Intake Id?
        new()
        {
            Id = new Guid("7579550f-a641-457d-ba59-47e31c87dbee"),
            OwnerId = new Guid("88e3ffd5-0de9-487b-a053-da87bcca62cf"), // Override manually
            PropertyName = "Sweet hills",
            CostPerDay = 300,
            AirbnbReference = Guid.NewGuid(),
            EstateType = Responses.Common.EstateTypeResponse.Apartment,
            IsAvailable = false,
            Rentals = new List<RentalResponse>()
            {
                new()
                {
                    EstateId = new Guid("7579550f-a641-457d-ba59-47e31c87dbee"),
                    RentType = RentType.ShortTerm,
                    StartDate = DateOnly.FromDateTime(DateTime.Today),
                    EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
                }
            },
            Address = new()
            {
                Country = "Poland",
                City = "Gdańsk",
                PostalCode = "80-100",
                StreetName = "Slodka",
                PropertyNumber = 21
            }
        };

    public static IEnumerable<AirbnbEstateResponse> GetAllAirbnbEstatesResponse() =>
        new List<AirbnbEstateResponse>()
        {
            new AirbnbEstateResponse()
            {
                Id = new Guid("7579550f-a641-457d-ba59-47e31c87dbee"),
                OwnerId = new Guid("88e3ffd5-0de9-487b-a053-da87bcca62cf"), // Override manually
                PropertyName = "Sweet hills",
                CostPerDay = 300,
                AirbnbReference = Guid.NewGuid(),
                EstateType = Responses.Common.EstateTypeResponse.Apartment,
                IsAvailable = false,
                Rentals = new List<RentalResponse>()
                {
                    new()
                    {
                        EstateId = new Guid("7579550f-a641-457d-ba59-47e31c87dbee"),
                        RentType = RentType.ShortTerm,
                        StartDate = DateOnly.FromDateTime(DateTime.Today),
                        EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
                    }
                },
                Address = new()
                {
                    Country = "Poland",
                    City = "Gdańsk",
                    PostalCode = "80-100",
                    StreetName = "Slodka",
                    PropertyNumber = 21
                }
            },
            new AirbnbEstateResponse()
            {
                Id = new Guid("37a9ad2f-cc61-40ac-8875-f341ba13d3a4"),
                OwnerId = new Guid("88e3ffd5-0de9-487b-a053-da87bcca62cf"), // Override manually
                PropertyName = "Cozy Retreat",
                CostPerDay = 250,
                AirbnbReference = Guid.NewGuid(),
                EstateType = Responses.Common.EstateTypeResponse.House,
                IsAvailable = true,
                Rentals = new List<RentalResponse>
                {
                    new RentalResponse
                    {
                        EstateId = new Guid("37a9ad2f-cc61-40ac-8875-f341ba13d3a4"),
                        RentType = RentType.LongTerm,
                        StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(10)),
                        EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30))
                    }
                },
                Address = new AddressResponse
                {
                    Country = "France",
                    City = "Paris",
                    PostalCode = "75001",
                    StreetName = "Champs-Élysées",
                    PropertyNumber = 10
                }
            }
        };
}
