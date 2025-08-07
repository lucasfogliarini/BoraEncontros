using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Options;

namespace BoraEncontros.GoogleCalendar;

internal class GoogleCalendarService(IDataStore dataStore, IOptions<GoogleCalendarSettings> settings) : ICalendarService
{
    public async Task<IEnumerable<EventResponse>> GetEventsAsync(string user, EventRequestFilter? eventRequestFilter = null, CancellationToken cancellationToken = default)
    {
        eventRequestFilter ??= new EventRequestFilter();
        var calendarService = await InitializeCalendarServiceAsync(user);
        var request = calendarService.Events.List(eventRequestFilter.CalendarId);
        request.TimeMinDateTimeOffset = eventRequestFilter.TimeMin ?? DateTime.Now;
        request.TimeMaxDateTimeOffset = eventRequestFilter.TimeMax ?? DateTime.Now.AddYears(1);
        request.Q = eventRequestFilter.Query;
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        //var eventsCount = eventsFilter.FavoritesCount ? await EventsCountAsync(user) : null;
        var events = await request.ExecuteAsync(cancellationToken);

        var eventItems = events.Items.AsEnumerable();

        var eventsResponse = eventItems.Where(i => i.Visibility == "public").Select(i => (EventResponse)i);
        if (eventRequestFilter.HasTicket.GetValueOrDefault())
            eventsResponse = eventsResponse.Where(e => e.TicketUrl != null);
        return eventsResponse;
    }

    public async Task<EventResponse> CreateAsync(string user, EventRequest eventRequest, CancellationToken cancellationToken = default)
    {
        var calendarService = await InitializeCalendarServiceAsync(user);
        await InitializeCalendarServiceAsync(user);
        var request = calendarService.Events.Insert(eventRequest, eventRequest.CalendarId);
        request.ConferenceDataVersion = 1;
        var googleEvent = await request.ExecuteAsync(cancellationToken);
        return (EventResponse)googleEvent;
    }

    private async Task<CalendarService> InitializeCalendarServiceAsync(string user)
    {
        var googleCalendarSettings = settings.Value;
        var userCredential = await GetUserCredentialAsync(user, googleCalendarSettings);

        return new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = userCredential,
            ApplicationName = googleCalendarSettings.ApplicationName ?? "Bora Encontros",
        });
    }
    private async Task<UserCredential> GetUserCredentialAsync(string user, GoogleCalendarSettings googleCalendarSettings)
    {
        UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                googleCalendarSettings.GetClientSecrets(),
                googleCalendarSettings.Scopes,
                user,
                CancellationToken.None,
                dataStore);

        return credential;
    }
}