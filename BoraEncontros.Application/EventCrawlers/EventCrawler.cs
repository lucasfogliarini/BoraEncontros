using AngleSharp;
using AngleSharp.Dom;
using System.Globalization;

namespace BoraEncontros.Application.EventCrawlers;

public abstract class EventCrawler
{
    public string? EventsAbsolutePath { get; set; }

    public async Task<IEnumerable<CrawledEvent>> CrawlEventsAsync(DateTime startDate, DateTime endDate, string titlePrefix, CancellationToken cancellationToken = default)
    {
        var document = await GetHtmlDocumentAsync(cancellationToken);
        IEnumerable<CrawledEvent> eventsCrawled = ExtractEvents(document);
        eventsCrawled = eventsCrawled
            .Where(e => e.DateTime >= startDate && e.DateTime <= endDate)
            .OrderBy(e => e.DateTime);

        foreach (var @event in eventsCrawled)
        {
            @event.Title = $"{titlePrefix} {@event.Title}";
        }
        return eventsCrawled;
    }

    protected abstract List<CrawledEvent> ExtractEvents(IDocument document);

    private async Task<IDocument> GetHtmlDocumentAsync(CancellationToken cancellationToken = default)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);

        using var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(EventsAbsolutePath, cancellationToken);
        return await context.OpenAsync(req => req.Content(html));
    }

    protected static DateTime? ParseDateTime(string day, string MMMBr, string HHmm = "00:00")
    {
        Dictionary<string, string> monthMap = new()
            {
                { "jan", "Jan" },
                { "fev", "Feb" },
                { "mar", "Mar" },
                { "abr", "Apr" },
                { "mai", "May" },
                { "jun", "Jun" },
                { "jul", "Jul" },
                { "ago", "Aug" },
                { "set", "Sep" },
                { "out", "Oct" },
                { "nov", "Nov" },
                { "dez", "Dec" }
            };

        string MMM = monthMap[MMMBr.ToLower()];
        string format = "dd MMM HH:mm";
        string formattedDate = $"{day} {MMM} {HHmm}";

        if (DateTime.TryParseExact(formattedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dateTime))
        {
            if (dateTime < DateTime.Now)
                return dateTime.AddYears(1);
            return dateTime;
        }

        return null;
    }
}
