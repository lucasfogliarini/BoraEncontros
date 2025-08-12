using BoraEncontros.Application.Crawlers;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace BoraEncontros.Application.Calendars;

public class CrawlCaldeiraEventsQueryHandler(ILogger<CrawlCaldeiraEventsQueryHandler> logger) : IQueryHandler<CrawlCaldeiraEventsQuery, CaldeiraEventsResponse>
{
    public async Task<Result<CaldeiraEventsResponse>> Handle(CrawlCaldeiraEventsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var caldeiraCrawler = new CaldeiraCrawler(query.EventQuery);

            string message = $"Buscando eventos de '{caldeiraCrawler.EventsAbsolutePath}' do dia '{query.StartDate:dd/MM/yyyy}' até '{query.EndDate:dd/MM/yyyy}'.";
            logger.LogInformation(message);

            var titlePrefix = "👨‍💻";
            var eventsCrawled = await caldeiraCrawler.CrawlEventsAsync(query.StartDate, query.EndDate, titlePrefix, cancellationToken);

            logger.LogInformation($"{eventsCrawled.Count()} eventos encontrados.");

            var integrationEvents = eventsCrawled.Select(e => new EventCreatedIntegrationEvent
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

            return Result.Success(new CaldeiraEventsResponse(integrationEvents));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar eventos da Caldeira");
            return Result.Failure<CaldeiraEventsResponse>($"Erro ao buscar eventos: {ex.Message}");
        }
    }
}

public record CrawlCaldeiraEventsQuery(string EventQuery, DateTime StartDate, DateTime EndDate) : IQuery<CaldeiraEventsResponse>;

public record CaldeiraEventsResponse(IEnumerable<EventCreatedIntegrationEvent> Events);
