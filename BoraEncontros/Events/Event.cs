namespace BoraEncontros.Events;

public class Event
{
    public const string CalendarEmailDefault = "lucasfogliarini@gmail.com";
    public const string CalendarIdDefault = "e2f2f8cb6219100331249c78a6d0ad4b66992ddae690bee257d1654f867687a4@group.calendar.google.com";//primary

    public required string CalendarEmail { get; set; }
    public required string CalendarId { get; set; }
    public required string Title { get; set; }
    public required string EventLink { get; set; }
    public required DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
    public bool Public { get; set; } = true;
}
