namespace BoraEncontros.Events;

public class Event
{
    public required string CalendarEmail { get; set; } = "lucasfogliarini@gmail.com";
    public required string Title { get; set; }
    public required string EventLink { get; set; }
    public required DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
    public bool Public { get; set; } = true;
}
