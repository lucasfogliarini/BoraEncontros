using BoraEncontros.Events;
using BoraEncontros.Application;
using BoraEncontros.Application.Calendars;
using Microsoft.Azure.Functions.Worker;

namespace BoraEncontros.Workers.Consumers;

public class EventCreatedConsumer(ICommandHandler<CreateEventCommand> commandHandler)
{
    [Function(nameof(EventCreatedConsumer))]
    public async Task Run(
        [ServiceBusTrigger("event-created", Connection = "AzureServiceBusConnectionString")]
        Event eventCreated)
    {
        var command = new CreateEventCommand(eventCreated);
        await commandHandler.HandleAsync(command);
    }
}