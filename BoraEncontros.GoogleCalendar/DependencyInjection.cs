using BoraEncontros.GoogleCalendar;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddGoogleCalendarService(this IServiceCollection services)
    {
        services.AddOptions<GoogleCalendarSettings>()
                        .BindConfiguration(nameof(GoogleCalendarSettings))
                        .ValidateDataAnnotations()
                        .ValidateOnStart();
        services.AddScoped<ICalendarService, GoogleCalendarService>();
        return services;
    }
}
