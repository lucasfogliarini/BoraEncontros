namespace BoraEncontros.Application.Calendars;

public class EventCreatedIntegrationEvent : IIntegrationEvent
{
		public string? Title { get; set; }
		public string? Description { get; set; }
		public DateTimeOffset? Start { get; set; }
		public DateTimeOffset? End { get; set; }
		public string? Location { get; set; }
		public string? ImageUrl { get; set; }
		public string? EventLink { get; set; }
		public bool? Public { get; set; } = true;
}
