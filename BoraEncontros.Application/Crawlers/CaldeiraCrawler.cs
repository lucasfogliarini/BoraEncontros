using AngleSharp.Dom;

namespace BoraEncontros.Application.Crawlers;

public class CaldeiraCrawler : BoraCrawler
{
    const string CALDEIRA_DOMAIN = "https://institutocaldeira.org.br";

    /// <summary>
    /// A crawler class for extracting event information from a 'https://institutocaldeira.org.br/'
    /// </summary>
    /// <param name="eventsQuery">https://institutocaldeira.org.br/{eventsQuery}</param>
    public CaldeiraCrawler(string eventsQuery)
    {
        EventsAbsolutePath = $"{CALDEIRA_DOMAIN}/{eventsQuery}";
    }

    protected override List<CrawledEvent> ExtractEvents(IDocument document)
    {
        var events = document.QuerySelectorAll(".schedule-card");

        List<CrawledEvent> eventList = [];

        foreach (var card in events)
        {
            try
            {
                var linkElement = card.QuerySelector("a");
                var link = linkElement?.GetAttribute("href") ?? string.Empty;

                var titleElement = card.QuerySelector(".schedule-card__title");
                var title = titleElement?.TextContent.Trim() ?? "";

                var labels = card.QuerySelectorAll(".schedule-card__label").ToList();

                string? dateText = labels.Count > 0 ? labels[0].TextContent.Trim() : null; // "23/maio"                    

                if (!string.IsNullOrEmpty(dateText))
                {
                    var dateParts = dateText.Split('/');
                    var day = dateParts[0];
                    var monthBr = dateParts[1][..3];

                    string timeText = labels.Count > 1 ? labels[1].TextContent.Trim().ToLower().Replace("h", ":") : "00:00";
                    if (timeText.EndsWith(":"))
                        timeText += "00";

                    timeText = timeText.PadLeft(5, '0');
                    var dateTime = ParseDateTime(day, monthBr, timeText);

                    if (dateTime.HasValue)
                    {
                        string? location2 = labels.Count > 2 ? labels[2].TextContent.Trim() : null; // "Espaço Multiuso - Campus"
                        location2 = location2 == "Instituto Caldeira" ? "" : $" - {location2}";
                        eventList.Add(new CrawledEvent
                        {
                            Title = title,
                            EventLink = link.StartsWith("http") ? link : $"{CALDEIRA_DOMAIN}{link}",
                            DateTime = dateTime.Value,
                            Location = $"Instituto Caldeira - Porto Alegre, RS {location2}"
                        });
                    }
                }
            }
            catch
            {
                continue;
            }
        }

        return eventList;
    }
}
