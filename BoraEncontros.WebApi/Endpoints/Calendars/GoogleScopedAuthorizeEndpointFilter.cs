namespace BoraEncontros.WebApi.Endpoints.Calendars
{
    public class GoogleScopedAuthorizeEndpointFilter(string requiredScope) : Attribute, IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var httpContext = context.HttpContext;

            var claimsPrincipal = httpContext.User;
            var scopeClaim = claimsPrincipal.FindFirst("scope");

            if (scopeClaim == null || !scopeClaim.Value.Split(' ').Contains(requiredScope))
            {
                return Results.Forbid();
            }

            return await next(context);
        }
    }

}
