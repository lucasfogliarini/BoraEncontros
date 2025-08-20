using BoraEncontros.Application;
using BoraEncontros.Application.Crawlers;
using BoraEncontros.Events;
using Microsoft.Azure.Functions.Worker;

namespace BoraEncontros.Workers.EventCrawlers;

public class CaldeiraEventCrawler(IQueryHandler<CrawlCaldeiraEventsQuery, CaldeiraEventsResponse> queryHandler)
{
    const string EVENTS_QUERY = "/agenda/";

    [Function(nameof(CaldeiraEventCrawler))]
    [ServiceBusOutput("event-created", Connection = "AzureServiceBusConnectionString")]
    public async Task<IEnumerable<Event>> RunAsync([TimerTrigger("%CrawlerCron%", RunOnStartup = true)] TimerInfo timer)
    {
        DateTime startDate = DateTime.Today;
        DateTime endDate = startDate.AddDays(7);// timer.ScheduleStatus.Next;

        var crawlCaldeiraEventsQuery = new CrawlCaldeiraEventsQuery(EVENTS_QUERY, startDate, endDate);
        var caldeiraEventsResponse = await queryHandler.HandleAsync(crawlCaldeiraEventsQuery);
        if(caldeiraEventsResponse.IsFailure)
            throw new InvalidOperationException(caldeiraEventsResponse.Error);
        return caldeiraEventsResponse.Value.Events;
    }
}
