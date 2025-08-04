using Google.Apis.Calendar.v3;

namespace BoraEncontros.WebApi.Endpoints.Calendars;

internal sealed class AuthorizeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Routes.Calendars}2/Authorize", (string? redirectUrl) =>
        {
            if (redirectUrl == null)
            {
                return Results.NoContent();
            }
            return Results.Redirect(redirectUrl);
        })
        .WithTags(Routes.Calendars)
        .AddEndpointFilter(new GoogleScopedAuthorizeEndpointFilter(CalendarService.ScopeConstants.Calendar));
    }
}


