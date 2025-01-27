using System.Net;
using System.Text.Json;

namespace Task.Api.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async System.Threading.Tasks.Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static System.Threading.Tasks.Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails
        {
            Success = false,
            StatusCode = context.Response.StatusCode,
            Messages = ["Internal Server Error."]
        }, options));
    }
}

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string[] Messages { get; set; }
    public bool Success { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}