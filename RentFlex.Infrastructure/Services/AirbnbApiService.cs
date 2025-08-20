using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Models;
using RentFlex.Domain.entities;

namespace RentFlex.Infrastructure.Services;
public class AirbnbApiService(HttpClient httpClient, ILogger<AirbnbApiService> logger) : IAirbnbApiService
{
    private readonly HttpClient httpClient = httpClient;
    private readonly ILogger<AirbnbApiService> logger = logger;

    public async Task<string> Test()
    {
        try
        {
            logger.LogInformation("Testing ApiClient");
            var byteArray = Encoding.ASCII.GetBytes("user:password");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(nameof(AuthenticationSchemes.Basic), Convert.ToBase64String(byteArray));
            var apiResponse = await httpClient.GetStringAsync("/api/hello");
            return apiResponse;
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, "Error While testing");
            throw;
        }
    }

    public async Task<IEnumerable<Estate>> GetAllEstatesAsync(Guid userReference)
    {
        try
        {
            var estates = await httpClient.GetFromJsonAsync<IEnumerable<Estate>>($"/airbnb/{userReference}/estates");
            return estates ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching all Estates from Airbnb");
            throw;
        }
    }

    public async Task<Guid> CreateEstateAsync(Guid userReference, EstateDto estate)
    {
        try
        {
            var apiResponse = await httpClient.PostAsJsonAsync($"/airbnb/{userReference}/estates",
                estate);

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var estateId = await apiResponse.Content.ReadAsStringAsync();
                return Guid.Parse(estateId);
            }

            return Guid.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while Creating Estate for Airbnb");
            throw;
        }
    }

    public async Task<bool> UpdateEstateAsync(Guid airbnbReference, EstateDto estate)
    {
        try
        {
            var apiResponse = await httpClient.PatchAsJsonAsync($"/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates",
                estate);

            return apiResponse.StatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating Estate from Airbnb");
            throw;
        }
    }

    public async Task<bool> DeleteEstateAsync(Guid airbnbReference)
    {
        try
        {
            var apiResponse = await httpClient.DeleteAsync($"/airbnb/88e3ffd5-0de9-487b-a053-da87bcca62cf/estates/{airbnbReference}");
            return apiResponse.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting Estate from Airbnb");
            throw;
        }
    }

    public async Task<IEnumerable<RentalDto>> GetAllRentals(Guid userReference, Guid estateReference, CancellationToken cancellationToken)
    {
        try
        {
            var rentals = await httpClient.GetFromJsonAsync<IEnumerable<RentalDto>>($"/airbnb/{userReference}/estates/{estateReference}/rentals", cancellationToken);
            return rentals ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching Rentals from Airbnb");
            throw;
        }
    }
}
