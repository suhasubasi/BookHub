using System.Text.Json;
namespace BookHub.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Internal server error" }));
        }

    }
}
