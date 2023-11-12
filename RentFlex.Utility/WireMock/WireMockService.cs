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

        wireMockServer.ConfigureEndpoints();
        wireMockServer.ConfigureAirbnbEndpoints();
    }

    private static void ConfigureEndpoints(this WireMockServer wireMockServer)
    {
        wireMockServer.Given(Request.Create().WithPath("/api/hello").UsingGet())
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
    }
}
