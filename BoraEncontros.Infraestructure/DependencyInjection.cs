using BoraEncontros.Accounts.Repositories;
using BoraEncontros.GoogleCalendar;
using BoraEncontros.Infraestructure.DataStores;
using BoraEncontros.Infrastructure;
using Google.Apis.Util.Store;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext();
            services.AddRepositories();
        }

        public static IServiceCollection AddGoogleCalendarService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDataStore, GoogleDataStore>();
            serviceCollection.AddScoped<ICalendarService, GoogleCalendarService>();
            return serviceCollection;
        }
        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICalendarTokenRepository, CalendarTokenRepository>();
        }

        private static void AddDbContext(this IServiceCollection services)
        {
            services.AddDbContext<BoraEncontrosDbContext>(options => options.UseInMemoryDatabase(nameof(BoraEncontrosDbContext)));
        }
    }
}
