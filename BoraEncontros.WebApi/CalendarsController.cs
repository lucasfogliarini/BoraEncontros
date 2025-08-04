using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Mvc;

namespace BoraEncontros.WebApi
{
	[ApiController]
    [Route("[controller]")]
    public class CalendarsController : Controller
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
    }
}
