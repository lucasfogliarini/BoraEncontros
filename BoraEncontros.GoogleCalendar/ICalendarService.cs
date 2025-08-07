namespace BoraEncontros.GoogleCalendar;

public interface ICalendarService
{
    Task<IEnumerable<EventResponse>> GetEventsAsync(string user, EventRequestFilter? eventRequestFilter = null, CancellationToken cancellationToken = default);
    Task<EventResponse> CreateAsync(string user, EventRequest eventRequest, CancellationToken cancellationToken = default);
}

public class FakeCalendarService : ICalendarService
{
    public Task<IEnumerable<EventResponse>> GetEventsAsync(string user, EventRequestFilter? eventRequestFilter = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Enumerable.Empty<EventResponse>());
    }
    public Task<EventResponse> CreateAsync(string user, EventRequest eventRequest, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new EventResponse());
    }
}