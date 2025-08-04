using BoraEncontros.GoogleCalendar;

namespace BoraEncontros.WebApi.Endpoints.Calendars;

internal sealed class EventsEndpoint(ICalendarService calendarService) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Routes.Calendars}/{{user}}/events", async (string user, CancellationToken cancellationToken) =>
        {
            var events = calendarService.GetEventsAsync(user);
            return Results.Ok(events);
        })
        .WithTags(Routes.Calendars);
    }
}