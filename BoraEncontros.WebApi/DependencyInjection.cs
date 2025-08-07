using BoraEncontros.GoogleCalendar;
using BoraEncontros.Infrastructure;
using BoraEncontros.WebApi;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        //builder.Services.AddEndpoints();
        builder.Services.AddProblemDetails();
        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromSeconds(30)));
        });
        builder.Services.AddOpenApi();
        builder.AddGoogleCalendar();
        builder.AddHealthChecks();

    }
    public static void UseWebApi(this WebApplication app)
    {
        app.UseOutputCache();//precisa ser antes do MapEndpoints
        app.MapControllers();
        //app.MapEndpoints();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        app.UseHttpsRedirection();
        app.UseHealthChecks();
        //app.UseAuthorization();
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var endpointTypes = typeof(Program).Assembly
            .DefinedTypes
            .Where(type => !type.IsAbstract
                           && !type.IsInterface
                           && typeof(IEndpoint).IsAssignableFrom(type))
            .Select(type => ServiceDescriptor.Scoped(typeof(IEndpoint), type));

        services.TryAddEnumerable(endpointTypes);

        return services;
    }
    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        IEnumerable<IEndpoint> endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
    private static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddBoraEncontrosDbContextCheck();
    }
    private static void UseHealthChecks(this WebApplication app)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true, // Executa todos os health checks
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    applicationVersion = version,
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        data = entry.Value.Data
                    })
                });

                await context.Response.WriteAsync(result);
            }
        });
    }

    public static void AddGoogleCalendar(this WebApplicationBuilder builder)
    {
        builder.Services.AddGoogleCalendarService();
        builder.Services.AddCalendarTokenDataStore();

        //https://developers.google.com/api-client-library/dotnet/guide/aaa_oauth#web-applications-asp.net-core-3
        builder.Services
            .AddAuthentication(o =>
            {
                // This forces challenge results to be handled by Google OpenID Handler, so there's no
                // need to add an AccountController that emits challenges for Login.
                o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                // This forces forbid results to be handled by Google OpenID Handler, which checks if
                // extra scopes are required and does automatic incremental auth.
                o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogleOpenIdConnect(options =>
            {
                var serviceProvider = builder.Services.BuildServiceProvider();
                var googleCalendarSettings = serviceProvider.GetRequiredService<IOptions<GoogleCalendarSettings>>().Value;

                options.ClientId = googleCalendarSettings.ClientId;
                options.ClientSecret = googleCalendarSettings.ClientSecret;
                options.Events.OnTokenValidated += async (ctx) =>
                {
                    var dataStore = serviceProvider.GetRequiredService<IDataStore>();
                    TokenResponse tokenResponse = new()
                    {
                        IssuedUtc = DateTime.UtcNow,
                        AccessToken = ctx.TokenEndpointResponse.AccessToken,
                        RefreshToken = ctx.TokenEndpointResponse.RefreshToken,
                        ExpiresInSeconds = long.Parse(ctx.TokenEndpointResponse.ExpiresIn),
                    };
                    string email = ctx.Principal.FindFirst(ClaimTypes.Email).Value;
                    await dataStore.StoreAsync(email, tokenResponse);
                };
            });
    }
}