namespace HBlog.Api.Middlewares;

public class CorrelationMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<CorrelationMiddleware> _logger;
	private const string CorrelationIdHeaderName = "X-Correlation-ID";

	public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		// Try to get the correlation ID from the request header.
		var correlationId = GetOrGenerateCorrelationId(context);

		// Add the correlation ID to the logging scope.
		// Any logs generated within this scope will have the CorrelationId property.
		using (_logger.BeginScope("{@CorrelationId}", correlationId))
		{
			// Add the correlation ID to the response headers.
			// This allows the client to see the ID for its request.
			context.Response.OnStarting(() =>
			{
				if (!context.Response.Headers.ContainsKey(CorrelationIdHeaderName))
				{
					context.Response.Headers.Add(CorrelationIdHeaderName, correlationId);
				}
				return Task.CompletedTask;
			});

			// Pass control to the next middleware in the pipeline.
			await _next(context);
		}
	}

	private static string GetOrGenerateCorrelationId(HttpContext context)
	{
		if (context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationIdValues) &&
			correlationIdValues.FirstOrDefault() is { Length: > 0 } correlationId)
		{
			return correlationId;
		}

		return Guid.NewGuid().ToString();
	}
}
