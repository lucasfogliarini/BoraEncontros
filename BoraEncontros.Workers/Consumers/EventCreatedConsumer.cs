using BoraEncontros.Application;
using BoraEncontros.Application.Calendars;
using Microsoft.Azure.Functions.Worker;

namespace BoraEncontros.Workers.Consumers;

public class EventCreatedConsumer(IIntegrationEventHandler<EventCreatedIntegrationEvent> integrationEventHandler)
{
    [Function(nameof(EventCreatedConsumer))]
    public async Task Run(
        [ServiceBusTrigger("event-created", Connection = "AzureServiceBusConnectionString")]
        EventCreatedIntegrationEvent eventCreated)
    {
        await integrationEventHandler.HandleAsync(eventCreated);
    }
}