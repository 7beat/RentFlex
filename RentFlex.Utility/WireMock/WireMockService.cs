using Newtonsoft.Json;
using RentFlex.Utility.WireMock.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace RentFlex.Utility.WireMock;
public static class WireMockService
{
    private static WireMockServer wireMockServer = null!;
    public static void Start()
    {
        Console.WriteLine("Starting WireMock server!");
        var settings = new WireMockServerSettings
        {
            Port = 5000,
        };

        wireMockServer = WireMockServer.Start(settings);

#if DEBUG
        var authHeader = "Basic " + Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes("user:password"));

        wireMockServer.ConfigureTestEndpoints(authHeader);
#endif
    }

    private static void ConfigureTestEndpoints(this WireMockServer wireMockServer, string authHeader)
    {
        wireMockServer.Given(Request.Create().WithPath("/api/hello").UsingGet().WithHeader("Authorization", authHeader))
              .RespondWith(Response.Create().WithStatusCode(200).WithBody("world"));

        var testResponse = new TestResponse();
        var jsonResponse = JsonConvert.SerializeObject(testResponse);
        wireMockServer.Given(Request.Create().WithPath("/api/rentals").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(jsonResponse));

    }

    private static void ConfigureAirbnbEndpoints(this WireMockServer wireMockServer)
    {
        // GetSingleAirbnbEstate, airbnbReference/estates/estateId
        var jsonSingleAirbnbResponse = JsonConvert.SerializeObject(ResponseGenerator.GetSingleAirbnbEstateResponse(), Formatting.Indented);
        wireMockServer.Given(Request.Create().WithPath($"/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates/7579550f-a641-457d-ba59-47e31c87dbee").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(jsonSingleAirbnbResponse));

        // GetAllAirbnbEstates
        var jsonAllAirbnbEstates = JsonConvert.SerializeObject(ResponseGenerator.GetAllAirbnbEstatesResponse(), Formatting.Indented);
        wireMockServer.Given(Request.Create().WithPath($"/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(jsonAllAirbnbEstates));

        // CreateAirbnbEstate
        wireMockServer.Given(Request.Create().WithPath("/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(201).WithBody(Guid.NewGuid().ToString()));

        // UpdateAirbnbEstate
        wireMockServer.Given(Request.Create().WithPath("/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates/7579550f-a641-457d-ba59-47e31c87dbee").UsingPatch())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody("Estate with Id = 7579550f-a641-457d-ba59-47e31c87dbee was succesfully updated"));

        // DeleteAirbnbEstate
        wireMockServer.Given(Request.Create().WithPath("/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates/7579550f-a641-457d-ba59-47e31c87dbee").UsingDelete())
            .RespondWith(Response.Create().WithStatusCode(204).WithBody("Estate with Id = 7579550f-a641-457d-ba59-47e31c87dbee was succesfully deleted"));
    }

    // For simplicity both airbnb/booking references will be the same for preview model
    public static void ConfigureEndpoints(string userReference, string estateReference)
    {
        // GetSingleAirbnbEstate
        var jsonSingleAirbnbResponse = JsonConvert.SerializeObject(ResponseGenerator.GetSingleAirbnbEstateResponse(), Formatting.Indented);
        wireMockServer.Given(Request.Create().WithPath($"/airbnb/{userReference}/estates/{estateReference}").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(jsonSingleAirbnbResponse));

        // GetAllAirbnbEstates
        var jsonAllAirbnbEstates = JsonConvert.SerializeObject(ResponseGenerator.GetAllAirbnbEstatesResponse(), Formatting.Indented);
        wireMockServer.Given(Request.Create().WithPath($"/airbnb/{userReference}/estates").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(jsonAllAirbnbEstates));

        // CreateAirbnbEstate
        wireMockServer.Given(Request.Create().WithPath($"/airbnb/{userReference}/estates").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(201).WithBody(Guid.NewGuid().ToString()));

        // UpdateAirbnbEstate
        wireMockServer.Given(Request.Create().WithPath($"/airbnb/{userReference}/estates/{estateReference}").UsingPatch())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"Estate with Id: {estateReference} was succesfully updated"));

        // DeleteAirbnbEstate
        wireMockServer.Given(Request.Create().WithPath($"/airbnb/{userReference}/estates/{estateReference}").UsingDelete())
            .RespondWith(Response.Create().WithStatusCode(204).WithBody("Estate with Id = 7579550f-a641-457d-ba59-47e31c87dbee was succesfully deleted"));

        //// Rentals Single
        //var jsonSingleRentalResponse = JsonConvert.SerializeObject(ResponseGenerator.GetSingleAirbnbRentalResponse(), Formatting.Indented);
        //wireMockServer.Given(Request.Create().WithPath($"/airbnb/{userReference}/estates/{estateReference}/rentals").UsingGet())
        //    .RespondWith(Response.Create().WithStatusCode(200).WithBody(jsonSingleRentalResponse));

        // Rentals All
        var jsonAllRentalsResponse = JsonConvert.SerializeObject(ResponseGenerator.GetAllAirbnbRentalsResponse(), Formatting.Indented);
        wireMockServer.Given(Request.Create().WithPath($"/airbnb/{userReference}/estates/{estateReference}/rentals").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(jsonAllRentalsResponse));
    }
}
