using BoraEncontros.Accounts.Repositories;
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
        public static void AddCalendarTokenDataStore(this IServiceCollection services)
        {
            services.AddScoped<IDataStore, CalendarTokenDataStore>();
        }
        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICalendarTokenRepository, CalendarTokenRepository>();
        }

        private static void AddDbContext(this IServiceCollection services)
        {
            services.AddDbContext<BoraEncontrosDbContext>(options => options.UseInMemoryDatabase(nameof(BoraEncontrosDbContext)));
        }
    }
}
