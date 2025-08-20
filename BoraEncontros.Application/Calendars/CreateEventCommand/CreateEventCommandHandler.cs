using BoraEncontros.GoogleCalendar;
using BoraEncontros.Events;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace BoraEncontros.Application.Calendars;

public class CreateEventCommandHandler(ICalendarService calendarService, ILogger<CreateEventCommandHandler> logger) : ICommandHandler<CreateEventCommand>
{
    public async Task<Result> HandleAsync(CreateEventCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Integrando o evento '{Title}' do dia {Start} na agenda '{Agenda}'",
            command.Event.Title,
            command.Event.Start,
            command.Event.CalendarEmail
        );

        var eventRequest = new EventRequest
        {
            Title = command.Event.Title,
            EventLink = command.Event.EventLink,
            Description = command.Event.EventLink,
            Location = command.Event.Location,
            Start = command.Event.Start,
            End = command.Event.End ?? command.Event.Start.AddHours(1),
            Public = command.Event.Public,
        };

        var calendarEventCreated = await calendarService.CreateAsync(command.Event.CalendarEmail, eventRequest, cancellationToken);
        if(calendarEventCreated != null)
            logger.LogInformation("Evento '{Title}' criado com sucesso na agenda {Agenda}", command.Event.Title, command.Event.CalendarEmail);

        return Result.Success();
    }
}

public record CreateEventCommand(Event Event) : ICommand;


