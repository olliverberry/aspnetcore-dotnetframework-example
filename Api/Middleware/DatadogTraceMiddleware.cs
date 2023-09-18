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
                    scope.Span.SetTag("http.host", context.Request.Host.Host);
                    scope.Span.SetTag("http.path_group", context.Request.Path.Value);
                    scope.Span.SetTag("http.method", context.Request.Method);
                    scope.Span.SetTag("http.status_code", context.Response.StatusCode);
                    scope.Span.SetTag("duration", stopwatch.ElapsedTicks);
                    scope.Span.SetTag("span.kind", "server");
                    scope.Span.ResourceName = $"{context.Request.Method} {context.Request.Path.Value}";
                }
            }
        }
    }
}
