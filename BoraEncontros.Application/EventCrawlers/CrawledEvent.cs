namespace BoraEncontros.Application.EventCrawlers;

public class CrawledEvent
{
    public required string Title { get; set; }
    public required string EventLink { get; set; }
    public required DateTime DateTime { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
	public string? Price { get; set; }
}
