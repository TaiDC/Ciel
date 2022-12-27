using Ciel.API.Core;
using System.Text.Json;

namespace Ciel.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }
    private Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex.Message);

        var errors = new List<string>() { ex.Message };

        var code = StatusCodes.Status500InternalServerError;
        code = ex switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,
            //NotFoundException => StatusCodes.Status404NotFound,
            //BadRequestException => StatusCodes.Status400BadRequest,
            //UnprocessableRequestException => StatusCodes.Status422UnprocessableEntity,
            _ => code
        };

        var result = JsonSerializer.Serialize(ApiResult<string>.Failure(errors));
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        return context.Response.WriteAsync(result);
    }
}