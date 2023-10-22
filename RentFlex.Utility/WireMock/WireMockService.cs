using Newtonsoft.Json;
using RentFlex.Utility.WireMock.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace RentFlex.Utility.WireMock;
public static class WireMockService
{
    private static WireMockServer wireMockServer;
    public static void Start()
    {
        Console.WriteLine("Starting WireMock server!");
        var settings = new WireMockServerSettings
        {
            Port = 5000,
        };

        wireMockServer = WireMockServer.Start(settings);

        wireMockServer.ConfigureEndpoints();
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
}
