using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace RentFlex.Infrastructure.Services;
public class AirbnbService
{
    private static AsyncCircuitBreakerPolicy circutBreakerPolicy;
    private static AsyncRetryPolicy retryPolicy;

    private readonly HttpClient httpClient;

    public AirbnbService()
    {
        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(2, sd => TimeSpan.FromSeconds(20));

    }

    //public AirbnbService()
    //{
    //    httpClient = new HttpClient();
    //}
}
