using BoraEncontros.Accounts.Repositories;
using BoraEncontros.GoogleCalendar;
using CSharpFunctionalExtensions;

namespace BoraEncontros.Application.Calendars;

public class GetEventsCommandHandler(ICalendarService calendarService, ICalendarTokenRepository calendarTokenRepository) : IQueryHandler<GetEventsQuery, GetEventsResponse>
{
    public async Task<Result<GetEventsResponse>> Handle(GetEventsQuery query, CancellationToken cancellationToken = default)
    {
        var calendarToken = await calendarTokenRepository.GetByUsernameAsync(query.Username);

        if (calendarToken is null)
            return Result.Failure<GetEventsResponse>("Calendário não autorizado.");

        try
        {
            var events = await calendarService.GetEventsAsync(calendarToken.Account.Email);
            var response = new GetEventsResponse(events);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<GetEventsResponse>($"Erro ao buscar eventos: {ex.Message}");
        }
    }

}

public record GetEventsQuery(string Username) : IQuery<GetEventsResponse>;

public record GetEventsResponse(IEnumerable<EventResponse> Events);
