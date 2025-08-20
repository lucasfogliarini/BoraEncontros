using BoraEncontros.GoogleCalendar;
using Microsoft.Extensions.Logging;

namespace BoraEncontros.Application.Calendars;

public class EventCreatedIntegrationEventHandler(ICalendarService calendarService, ILogger<EventCreatedIntegrationEventHandler> logger) : IIntegrationEventHandler<EventCreatedIntegrationEvent>
{
    public async Task HandleAsync(EventCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Integrando o evento '{Title}' do dia {Start} na agenda '{Agenda}'",
            integrationEvent.Title,
            integrationEvent.Start,
            integrationEvent.CalendarEmail
        );

        var eventRequest = new EventRequest
        {
            Title = integrationEvent.Title,
            EventLink = integrationEvent.EventLink,
            Description = integrationEvent.EventLink,
            Location = integrationEvent.Location,
            Start = integrationEvent.Start,
            End = integrationEvent.End ?? integrationEvent.Start.AddHours(1),
            Public = integrationEvent.Public,
        };

        var calendarEventCreated = await calendarService.CreateAsync(integrationEvent.CalendarEmail, eventRequest, cancellationToken);
        if(calendarEventCreated != null)
            logger.LogInformation("Evento '{Title}' criado com sucesso na agenda {Agenda}", integrationEvent.Title, integrationEvent.CalendarEmail);
    }
}


