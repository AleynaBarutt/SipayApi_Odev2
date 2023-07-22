using SipayApi.Middleware;

namespace SipayApi.Extensions
{
    public static class LoggingExtensions
    {
        // Extension metodu, ILogger arayüzüne eklenen metot
        // this ILogger: Bu, bu metodu ILogger arayüzüne eklediğimizi gösterir.
        // HttpContext context: Metodun alacağı HttpContext parametresi.
        public static void LogRequest(this ILogger logger, HttpContext context)
        {
            var request = context.Request;
            // ILogger'ın LogInformation metodu kullanılarak isteği loglayabiliriz.
            logger.LogInformation($"Request {request.Method}: {request.Path}");
        }
    }

    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
