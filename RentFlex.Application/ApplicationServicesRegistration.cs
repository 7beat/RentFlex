using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RentFlex.Application;
public static class ApplicationServicesRegistration
{
    public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
