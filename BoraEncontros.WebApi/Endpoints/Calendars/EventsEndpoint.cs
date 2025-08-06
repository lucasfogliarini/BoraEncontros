using BoraEncontros.Application;
using BoraEncontros.Application.Calendars;

namespace BoraEncontros.WebApi.Endpoints.Calendars;

internal sealed class EventsEndpoint : IEndpoint
{
    public async Task<IResult> GetEventsAsync(string username,
                                              IQueryHandler<GetEventsQuery, GetEventsResponse> queryHandler,
                                               CancellationToken cancellationToken = default)
    {
        var result = await queryHandler.Handle(new GetEventsQuery(username), cancellationToken);
        if (result.IsFailure)
            return Results.NotFound(result.Error);

        return Results.Ok(result.Value);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
        .MapGet($"{Routes.Calendars}/{{username}}/events", GetEventsAsync)
        .WithTags(Routes.Calendars);
    }
}