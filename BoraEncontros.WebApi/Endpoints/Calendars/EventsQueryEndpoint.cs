using BoraEncontros.GoogleCalendar;
using Microsoft.AspNetCore.Mvc;

namespace BoraEncontros.WebApi.Endpoints.Calendars;

internal sealed class EventsQueryEndpoint(ICalendarService calendarService) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Routes.Calendars}/events", async (string user, [FromBody] EventRequestFilter eventRequestFilter, CancellationToken cancellationToken) =>
        {
            var events = calendarService.GetEventsAsync(user, eventRequestFilter);
            return Results.Ok(events);
        })
        .WithTags(Routes.Calendars);
    }
}