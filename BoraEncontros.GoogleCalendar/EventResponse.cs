using Google.Apis.Calendar.v3.Data;

namespace BoraEncontros.GoogleCalendar;

public class EventResponse
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public string? Location { get; set; }
    public DateTime? Deadline { get; set; }
    public string? Chat { get; set; }
    public string? ConferenceUrl { get; set; }
    public string? TicketUrl { get; set; }
    public string? TicketDomain { get; set; }
    public decimal Discount { get; set; }
    public string? SpotifyUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? YouTubeUrl { get; set; }
    public string[]? Attachments { get; set; }
    public string? GoogleEventUrl { get; set; }
    public IList<string>? Recurrence { get; set; }
    public bool Public { get; set; }

    public const string PRIVATE = "#private";
    public const string PRIVADO = "#privado";

    public static implicit operator EventResponse(Event @event)
    {
        return new EventResponse
        {
            Id = @event.Id,
            Title = @event.Summary,
            Description = @event.Description,
            Location = @event.Location,
            Start = @event.Start.DateTimeDateTimeOffset,
            End = @event.End.DateTimeDateTimeOffset,
            GoogleEventUrl = @event.HtmlLink,
            Public = @event.IsPublic(),
            Chat = @event.GetWhatsAppGroupChat(),
            ConferenceUrl = @event.GetConferenceUrl(),
            //Attendees = attendeeOutputs,
            TicketUrl = @event.GetTicketUrl(),
            TicketDomain = @event.GetTicketDomain(),
            SpotifyUrl = @event.GetSpotifyUrl(),
            Discount = @event.GetDiscount(),
            InstagramUrl = @event.GetInstagramUrl(),
            YouTubeUrl = @event.GetYouTubeUrl(),
            Attachments = @event.GetAttachments(),
            Deadline = @event.GetDeadLine(),
            Recurrence = @event.Recurrence,
        };
    }
}
