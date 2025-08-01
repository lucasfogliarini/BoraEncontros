using BoraEncontros.GoogleCalendar;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddGoogleCalendarService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICalendarService, GoogleCalendarService>();
        return serviceCollection;
    }
}
