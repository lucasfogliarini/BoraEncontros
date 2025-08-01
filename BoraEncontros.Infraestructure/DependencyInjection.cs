using BoraEncontros.GoogleCalendar;
using Google.Apis.Util.Store;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGoogleCalendarService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDataStore, AccountDataStore>();
            serviceCollection.AddScoped<ICalendarService, GoogleCalendarService>();
            return serviceCollection;
        }
    }
}
