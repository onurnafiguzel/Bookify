using Serilog.Context;

namespace Bookify.Api.Middleware;

public class ReuqestContextLoggingMiddleware
{
	private const string CorrelationIdHeaderName = "X-Correlation-Id";
	private readonly RequestDelegate next;

	public ReuqestContextLoggingMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public Task Invoke(HttpContext httpContext)
	{
		using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext)))
		{
			return next(httpContext);
		}
	}

	private static string GetCorrelationId(HttpContext httpContext)
	{
		httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);

		return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
	}
}
