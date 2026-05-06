using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HBlog.Infrastructure.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public ILogger<GlobalExceptionHandler> _logger { get; }
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }


        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            _logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}", httpContext.TraceIdentifier);
			
            (int statusCode, string title) = exception switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, "Argument Invalid Request"),
                BadHttpRequestException => (StatusCodes.Status400BadRequest, "Bad Request"),
                _ => (StatusCodes.Status500InternalServerError, "Internal server error.")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = exception.GetType().Name,
                Detail = httpContext.RequestServices
	                .GetRequiredService<IHostEnvironment>()
	                .IsDevelopment() ? exception.Message : null,
                Instance = httpContext.Request.Path
			};
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
            return true;

        }
    }
}
