using Microsoft.Extensions.Logging;

namespace BoraEncontros.Application.Calendars;

public class EventCreatedIntegrationEventHandler(ILogger<EventCreatedIntegrationEventHandler> logger) : IIntegrationEventHandler<EventCreatedIntegrationEvent>
{
    public async Task HandleAsync(EventCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {

    }
}


