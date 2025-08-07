using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace BoraEncontros.GoogleCalendar;

internal class GoogleCalendarSettingsHealthCheck(IOptions<GoogleCalendarSettings> options) : IHealthCheck
{
    private readonly GoogleCalendarSettings _settings = options.Value;

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(_settings.ApplicationName))
            errors.Add("ApplicationName is missing.");
        if (string.IsNullOrWhiteSpace(_settings.ClientId))
            errors.Add("ClientId is missing.");
        if (string.IsNullOrWhiteSpace(_settings.ClientSecret))
            errors.Add("ClientSecret is missing.");
        if (_settings.Scopes.Length == 0)
            errors.Add("ClientSecret is missing.");

        if (errors.Count == 0)
            return Task.FromResult(HealthCheckResult.Healthy("GoogleCalendarSettings are properly configured."));

        var description = string.Join(" ", errors);
        return Task.FromResult(HealthCheckResult.Unhealthy($"Invalid GoogleCalendarSettings: {description}"));
    }
}