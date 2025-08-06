using BoraEncontros.Accounts.Repositories;
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
        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICalendarTokenRepository, CalendarTokenRepository>();
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var boraEncontrosConnectionString = configuration.TryGetConnectionString();
            if(boraEncontrosConnectionString == null)
            {
                Console.WriteLine("No connection string found, using InMemoryDatabase for BoraEncontrosDbContext.");
                services.AddDbContext<BoraEncontrosDbContext>(options => options.UseInMemoryDatabase(nameof(BoraEncontrosDbContext)));
            }
            else
            {
                Console.WriteLine($"Using connection string for BoraEncontrosDbContext.");
                services.AddDbContext<BoraEncontrosDbContext>(options => options.UseSqlServer(boraEncontrosConnectionString));
            }
        }

        private static string? TryGetConnectionString(this IConfiguration configuration)
        {
            var boraEncontrosConnectionStringKey = "ConnectionStrings:BoraEncontros";
            Console.WriteLine($"Trying to get a database connectionString '{boraEncontrosConnectionStringKey}' from Configuration.");
            var connectionString = configuration[boraEncontrosConnectionStringKey];
            if (connectionString == null)
            {
                var message = $"{boraEncontrosConnectionStringKey} was not found! From builder.Configuration[{boraEncontrosConnectionStringKey}]";
                Console.WriteLine(message);
                throw new Exception(message);
            }

            Console.WriteLine();
            return connectionString;
        }
    }
}
