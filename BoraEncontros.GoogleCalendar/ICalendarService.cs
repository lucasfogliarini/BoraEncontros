namespace BoraEncontros.GoogleCalendar;

public interface ICalendarService
{
    Task<IEnumerable<EventResponse>> GetEventsAsync(string user, EventRequestFilter eventRequestFilter = null);
}