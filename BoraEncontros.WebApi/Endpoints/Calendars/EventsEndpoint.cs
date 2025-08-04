using BoraEncontros.Application;
using BoraEncontros.Application.Calendars;

namespace BoraEncontros.WebApi.Endpoints.Calendars;

internal sealed class EventsEndpoint(IQueryHandler<GetEventsQuery, GetEventsResponse> queryHandler) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Routes.Calendars}/{{username}}/events", async (string username, CancellationToken cancellationToken) =>
        {
            var result = await queryHandler.Handle(new GetEventsQuery(username), cancellationToken);
            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        })
        .WithTags(Routes.Calendars);
    }
}