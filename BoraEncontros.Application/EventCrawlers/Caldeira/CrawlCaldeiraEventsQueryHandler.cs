using BoraEncontros.Events;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace BoraEncontros.Application.EventCrawlers;

public class CrawlCaldeiraEventsQueryHandler(ILogger<CrawlCaldeiraEventsQueryHandler> logger) : IQueryHandler<CrawlCaldeiraEventsQuery, CaldeiraEventsResponse>
{
    public async Task<Result<CaldeiraEventsResponse>> HandleAsync(CrawlCaldeiraEventsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var caldeiraCrawler = new CaldeiraCrawler(query.EventQuery);

            string message = $"Buscando eventos de '{caldeiraCrawler.EventsAbsolutePath}' do dia '{query.StartDate:dd/MM/yyyy}' até '{query.EndDate:dd/MM/yyyy}'.";
            logger.LogInformation(message);

            var titlePrefix = "👨‍💻";
            var eventsCrawled = await caldeiraCrawler.CrawlEventsAsync(query.StartDate, query.EndDate, titlePrefix, cancellationToken);

            logger.LogInformation($"{eventsCrawled.Count()} eventos encontrados.");

            var events = eventsCrawled.Select(e => new Event
            {
                CalendarEmail = "lucasfogliarini@gmail.com",
                Title = e.Title,
                Start = new DateTimeOffset(e.DateTime),
                Description = e.Price,
                Location = e.Location,
                ImageUrl = e.ImageUrl,
                EventLink = e.EventLink,
                Public = true
            });

            return Result.Success(new CaldeiraEventsResponse(events));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar eventos da Caldeira");
            return Result.Failure<CaldeiraEventsResponse>($"Erro ao buscar eventos: {ex.Message}");
        }
    }
}

public record CrawlCaldeiraEventsQuery(string EventQuery, DateTime StartDate, DateTime EndDate) : IQuery<CaldeiraEventsResponse>;

public record CaldeiraEventsResponse(IEnumerable<Event> Events);
