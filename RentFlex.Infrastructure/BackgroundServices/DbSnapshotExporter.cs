using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.SqlServer.Dac;
using RentFlex.Application.Constants;
using RentFlex.Application.Contracts.Infrastructure.Services;

namespace RentFlex.Infrastructure.BackgroundServices;
internal class DbSnapshotExporter(IConfiguration configuration, IServiceScopeFactory scopeFactory, IFeatureManager featureManager, ILogger<DbSnapshotExporter> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && await featureManager.IsEnabledAsync(FeatureFlags.PersistDatabase))
        {
            try
            {
                var dacServices = new DacServices(configuration.GetConnectionString("DefaultConnection"));

                using var memoryStream = new MemoryStream();

                dacServices.ExportBacpac(memoryStream, "RentFlex");

                await UploadBacpacToBlobAsync(memoryStream, stoppingToken);

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning("An error occurred while saving Db snapshot: " + ex.Message);
                throw;
            }
        }
    }

    private async Task UploadBacpacToBlobAsync(Stream stream, CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();

        var storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
        stream.Position = 0;

        await storageService.PersistDbAsync(stream, stoppingToken);
    }
}
