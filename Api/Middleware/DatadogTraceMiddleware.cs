using Api.Extensions;
using Datadog.Trace;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class DatadogTraceMiddleware
    {
        private readonly RequestDelegate _next;

        public DatadogTraceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            using (var scope = Tracer.Instance.StartActive("aspnet_core.request"))
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    scope.Span.SetException(ex);
                    throw;
                }
                finally
                {
                    stopwatch.Stop();
                    scope.Span.SetHttpTagsFromContext(context)
                        .SetTag("span.kind", "server")
                        .SetTag("duration", stopwatch.ElapsedTicks);
                    scope.Span.ResourceName = $"{context.Request.Method} {context.Request.Path.Value}";
                }
            }
        }
    }
}
