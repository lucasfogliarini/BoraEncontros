namespace BoraEncontros.GoogleCalendar;
public class EventRequestFilter
{
    public string CalendarId { get; set; } = "primary";
    public bool FavoritesCount { get; set; }
    public DateTime? TimeMax { get; set; }
    public DateTime? TimeMin { get; set; }
	/// <summary>
	/// Free text search terms to find events that match these terms in the following
	///  fields: - summary - description - location - attendee's displayName - attendee's
    ///  email - workingLocationProperties.officeLocation.buildingId - workingLocationProperties.officeLocation.deskId
    ///  - workingLocationProperties.officeLocation.label - workingLocationProperties.customLocation.label
    ///  These search terms also match predefined keywords against all display title translations
    ///  of working location, out-of-office, and focus-time events.For example, searching
    ///  for "Office" or "Bureau" returns working location events of type officeLocation,
    ///  whereas searching for "Out of office" or "Abwesend" returns out-of-office events.
    ///  Optional.
	/// </summary>
	public string? Query { get; set; }
    public bool? HasTicket { get; set; }
}
