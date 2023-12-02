using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Models;
using RentFlex.Domain.entities;
using System.Text;

namespace RentFlex.Infrastructure.Services;
public class AirbnbService : IAirbnbService
{
    private static AsyncCircuitBreakerPolicy circutBreakerPolicy;
    private static AsyncRetryPolicy retryPolicy;

    private readonly HttpClient httpClient;
    private readonly ILogger<AirbnbService> logger;

    public AirbnbService(IHttpClientFactory httpClientFactory, ILogger<AirbnbService> logger)
    {
        httpClient = httpClientFactory.CreateClient("WireMockClient");
        this.logger = logger;

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
            logger.LogError(ex, "Error while fetching all Estates from Airbnb");
            throw;
        }

        return estates;
    }

    public async Task<Guid> CreateEstateAsync(Guid userReference, EstateDto estate)
    {
        Guid guid = new();

        try
        {
            await circutBreakerPolicy.ExecuteAsync(async () =>
            {
                var apiResponse = await httpClient.PostAsync($"/airbnb/{userReference}/estates",
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
            logger.LogError(ex, "Error while Creating Estate for Airbnb");
            throw;
        }

        return guid;
    }

    public async Task<bool> UpdateEstateAsync(Guid airbnbReference, EstateDto estate)
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
            logger.LogError(ex, "Error while updating Estate from Airbnb");
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
            logger.LogError(ex, "Error while deleting Estate from Airbnb");
            throw;
        }

        return result;
    }

    public async Task<IEnumerable<RentalDto>> GetAllRentals(Guid userReference, Guid estateReference)
    {
        List<RentalDto> rentals = new();
        try
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                var apiResponse = await httpClient.GetAsync($"/airbnb/{userReference}/estates/{estateReference}/rentals");
                var response = await apiResponse.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(response))
                {
                    rentals = JsonConvert.DeserializeObject<List<RentalDto>>(response)!;
                }
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching Rentals from Airbnb");
            throw;
        }

        return rentals;
    }
}
