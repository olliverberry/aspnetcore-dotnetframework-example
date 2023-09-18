using Datadog.Trace;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
    public static class DatadogExtensions
    {
        public static ISpan SetHttpTagsFromContext(this ISpan span, HttpContext context)
        {
            span.SetTag("http.host", context.Request.Host.Host)
                .SetTag("http.path_group", context.Request.Path.Value)
                .SetTag("http.method", context.Request.Method)
                .SetTag("http.status_code", context.Response.StatusCode);
            return span;
        }
    }
}
