namespace BoraEncontros.Application.Calendars;

public class EventCreatedIntegrationEvent : IIntegrationEvent
{
		public required string CalendarEmail { get; set; } = "lucasfogliarini@gmail.com";
		public required string Title { get; set; }
		public required DateTimeOffset Start { get; set; }
		public DateTimeOffset? End { get; set; }
		public string? Description { get; set; }
		public string? Location { get; set; }
		public string? ImageUrl { get; set; }
		public string? EventLink { get; set; }
		public bool Public { get; set; } = true;
}
