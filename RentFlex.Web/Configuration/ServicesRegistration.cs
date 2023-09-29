using RentFlex.Application;
using RentFlex.Infrastructure;

namespace RentFlex.Web.Configuration;

public static class ServicesRegistration
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterApplication(configuration);
        services.RegisterInfrastructure(configuration);
    }
}