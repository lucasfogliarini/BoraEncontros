using BoraEncontros.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BoraEncontros.Infraestructure;

public class DbContextHealthCheck<TDbContext>(TDbContext dbContext) : IHealthCheck where TDbContext : DbContext
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct)
    {
        var provider = dbContext.Database.ProviderName ?? "desconhecido";

        if (provider.Contains("InMemory", StringComparison.OrdinalIgnoreCase))
            return HealthCheckResult.Healthy("Provedor InMemory");

        var cancelAfterDelay = TimeSpan.FromSeconds(5);
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(cancelAfterDelay);

        if (await dbContext.Database.CanConnectAsync(timeoutCts.Token))
            return HealthCheckResult.Healthy($"Conexão OK com o provedor: {provider}");

        return HealthCheckResult.Unhealthy($"Não foi possível conectar ao provedor: {provider} dentro de {cancelAfterDelay.TotalSeconds} segundos");
    }
}