using Google.Apis.Calendar.v3.Data;

namespace BoraEncontros.GoogleCalendar;

public class EventRequest
{
    public required string Title { get; set; }
    public required DateTimeOffset Start { get; set; }
    public required DateTimeOffset End { get; set; }
    public required string EventLink { get; set; }
    public required string CalendarId { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public bool Public { get; set; }

    public static implicit operator Event(EventRequest eventRequest)
    {
        return new Event
        {
            Summary = eventRequest.Title,
            Description = eventRequest.Description,
            Location = eventRequest.Location,
            Start = new EventDateTime
            {
                DateTimeDateTimeOffset = eventRequest.Start,
            },            
            End = new EventDateTime
            {
                DateTimeDateTimeOffset = eventRequest.End,
            },
            Visibility = eventRequest.Public ? "public" : "private",
        };
    }
}
