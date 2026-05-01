namespace BookHub.Api.Middleware;

public class TenantMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // 1. READ — find companyId claim from the decoded JWT
        var companyIdClaim = context.User.Claims
            .FirstOrDefault(c => c.Type == "companyId")?.Value;

        // 2. STORE — attach it to the request
        if (companyIdClaim != null)
        {
            context.Items["companyId"] = Guid.Parse(companyIdClaim);
        }

        // 3. CONTINUE — pass request to next worker
        await _next(context);
    }
}

