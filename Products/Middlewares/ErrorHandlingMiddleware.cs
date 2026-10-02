

using Microsoft.AspNetCore.Http;
using Products.Domain.Exceptions.NotFound;

namespace Products.Api.Middlewares;
// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IProblemDetailsService _problemDetailsService;
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger,IProblemDetailsService problemDetailsService )
    {
        _next = next;
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex) {
            _logger.LogError("Unhandled Exception {TraceId}", httpContext.TraceIdentifier);
            var (statusCode, title) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                ArgumentNullException => (StatusCodes.Status400BadRequest, "Invalid Request"),
                //ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occured")
            };

            httpContext.Response.StatusCode = statusCode;

            await _problemDetailsService.WriteAsync(
                    new ProblemDetailsContext
                    {
                        HttpContext = httpContext,
                        ProblemDetails = {
                                Title = title,
                                Status = statusCode,
                                Detail = ex.Message
                        }
                    }

                );

        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
