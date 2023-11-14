using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Domain.entities;
using System.Text;

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

    public async Task<IEnumerable<Estate>> GetAllEstatesAsync(Guid userReference)
    {
        List<Estate> estates = new();

        try
        {
            await circutBreakerPolicy.ExecuteAsync(async () =>
            {
                var apiResponse = await httpClient.GetAsync($"/airbnb/{userReference}/estates");
                var response = await apiResponse.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(response))
                {
                    estates = JsonConvert.DeserializeObject<List<Estate>>(response)!;
                }
            });
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }

        return estates;
    }

    public async Task<Guid> CreateEstateAsync(Estate estate)
    {
        Guid guid = new();

        try
        {
            await circutBreakerPolicy.ExecuteAsync(async () =>
            {
                var apiResponse = await httpClient.PostAsync($"/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates",
                    new StringContent(JsonConvert.SerializeObject(estate), Encoding.UTF8, "application/json"));

                var response = await apiResponse.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(response))
                {
                    guid = Guid.Parse(response);
                }
            });
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }

        return guid;
    }

    public async Task<bool> UpdateEstateAsync(Guid airbnbReference, Estate estate)
    {
        bool result = false;

        try
        {
            await circutBreakerPolicy.ExecuteAsync(async () =>
            {
                var apiResponse = await httpClient.PatchAsync($"/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates",
                    new StringContent(JsonConvert.SerializeObject(estate), Encoding.UTF8, "application/json"));

                result = apiResponse.StatusCode == System.Net.HttpStatusCode.OK ? true : false;
            });
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }

        return result;
    }

    public async Task<bool> DeleteEstateAsync(Guid airbnbReference)
    {
        bool result = false;

        try
        {
            await circutBreakerPolicy.ExecuteAsync(async () =>
            {
                var apiResponse = await httpClient.DeleteAsync($"/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates/{airbnbReference}");

                result = apiResponse.StatusCode == System.Net.HttpStatusCode.NoContent ? true : false;
            });
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }

        return result;
    }
}
