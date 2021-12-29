using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Services.Middlewares
{
    public class RequestLoggerMiddleware
    {
        private readonly ILogger<RequestLoggerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var paramLst = new object[] { context.Request.Method, context.Request.Path, context.Request.QueryString,
                context.Connection.RemoteIpAddress, context.Connection.LocalIpAddress };
            using (_logger.BeginScope("{Method} {Path} {Query} {RemoteIp} {LocalIp}", paramLst))
            {
                await _next(context);
            }
        }
    }
}
