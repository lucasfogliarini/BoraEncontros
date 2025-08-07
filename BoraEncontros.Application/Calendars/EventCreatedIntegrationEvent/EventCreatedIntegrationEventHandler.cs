using BoraEncontros.GoogleCalendar;
using Microsoft.Extensions.Logging;

namespace BoraEncontros.Application.Calendars;

public class EventCreatedIntegrationEventHandler(ICalendarService calendarService, ILogger<EventCreatedIntegrationEventHandler> logger) : IIntegrationEventHandler<EventCreatedIntegrationEvent>
{
    public async Task HandleAsync(EventCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var eventRequest = new EventRequest
        {
            Title = integrationEvent.Title,
            Description = integrationEvent.Description,
            Location = integrationEvent.Location,
            Start = integrationEvent.Start,
            End = integrationEvent.End ?? integrationEvent.Start.AddHours(1),
            Public = integrationEvent.Public,
        };
        await calendarService.CreateAsync(integrationEvent.CalendarEmail, eventRequest, cancellationToken);
    }
}


