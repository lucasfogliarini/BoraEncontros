using BoraEncontros.Application;
using BoraEncontros.Application.Calendars;
using Microsoft.Azure.Functions.Worker;

namespace BoraEncontros.Workers;

public class EventCreatedConsumer(IQueryHandler<GetEventsQuery, GetEventsResponse> queryHandler, IIntegrationEventHandler<EventCreatedIntegrationEvent> integrationEventHandler)
{
    [Function(nameof(EventCreatedConsumer))]
    public async Task Run(
        [ServiceBusTrigger("event-created", Connection = "AzureServiceBusConnectionString")]
        EventCreatedIntegrationEvent eventCreated)
    {
         var events = await queryHandler.Handle(new GetEventsQuery("lucasfogliarini"));
        await integrationEventHandler.HandleAsync(eventCreated);
    }
}