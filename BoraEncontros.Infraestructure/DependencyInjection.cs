using BoraEncontros.Accounts.Repositories;
using BoraEncontros.Infraestructure;
using BoraEncontros.Infraestructure.DataStores;
using BoraEncontros.Infrastructure;
using Google.Apis.Util.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
        }
        public static void AddCalendarTokenDataStore(this IServiceCollection services)
        {
            services.AddScoped<IDataStore, CalendarTokenDataStore>();
        }

        public static void AddBoraEncontrosDbContextCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddCheck<DbContextHealthCheck<BoraEncontrosDbContext>>(nameof(BoraEncontrosDbContext));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICalendarTokenRepository, CalendarTokenRepository>();
        }
        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var boraEncontrosConnectionStringKey = "BoraEncontros";
            Console.WriteLine($"Trying to get a database connectionString '{boraEncontrosConnectionStringKey}' from Configuration.");
            var boraEncontrosConnectionString = configuration.GetConnectionString(boraEncontrosConnectionStringKey);
            if (boraEncontrosConnectionString == null)
            {
                Console.WriteLine("BoraEncontros ConnectionString NOT found, using InMemoryDatabase for BoraEncontrosDbContext.");
                services.AddDbContext<BoraEncontrosDbContext>(options => options.UseInMemoryDatabase(nameof(BoraEncontrosDbContext)));
            }
            else
            {
                Console.WriteLine($"Using BoraEncontros ConnectionString for BoraEncontrosDbContext.");
                services.AddDbContext<BoraEncontrosDbContext>(options => options.UseSqlServer(boraEncontrosConnectionString));
            }
        }
    }
}
