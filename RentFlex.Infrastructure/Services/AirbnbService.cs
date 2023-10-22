using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RentFlex.Application.Contracts.Infrastructure.Services;

namespace RentFlex.Infrastructure.Services;
public class AirbnbService : IAirbnbService
{
    private static AsyncCircuitBreakerPolicy circutBreakerPolicy;
    private static AsyncRetryPolicy retryPolicy;

    private readonly HttpClient httpClient;

    public AirbnbService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("WireMockClient");

        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(2, sd => TimeSpan.FromSeconds(20));

        circutBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30));
    }

    public async Task Test()
    {
        try
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                var apiResponse = await httpClient.GetAsync("/api/hello");
                var response = await apiResponse.Content.ReadAsStringAsync();
                Console.WriteLine(response);
                // Deserialize response to object
            });
        }
        catch (Exception ex) //when (ex.InnerException != null)
        {

            throw;
        }

    }
}
