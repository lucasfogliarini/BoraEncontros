using BoraEncontros.Application;
using BoraEncontros.Application.Calendars;
using BoraEncontros.GoogleCalendar;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BoraEncontros.WebApi.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class CalendarsController(IQueryHandler<GetEventsQuery, GetEventsResponse> queryHandler) : Controller
    {
        [HttpGet("authorize")]
        [GoogleScopedAuthorize(CalendarService.ScopeConstants.Calendar)]
        public IActionResult Authorize(string? redirectUrl)
        {
            if (redirectUrl == null)
            {
                return NoContent();
            }
            return Redirect(redirectUrl);
        }

        [HttpGet("{username}/events")]
        [OutputCache(Duration = 5)]
        public async Task<IActionResult> GetEventsAsync(string username, [FromQuery] EventRequestFilter eventsFilterQuery, [FromBody] EventRequestFilter? eventsFilterBody = null, CancellationToken cancellationToken = default)
        {
            var eventsFilter = eventsFilterBody ?? eventsFilterQuery;
            var result = await queryHandler.Handle(new GetEventsQuery(username, eventsFilter), cancellationToken);
            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value.Events);
        }
    }
}
