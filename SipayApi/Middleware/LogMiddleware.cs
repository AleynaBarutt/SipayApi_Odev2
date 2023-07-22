using SipayApi.Extensions;
using System;
using static System.Net.WebRequestMethods;

namespace SipayApi.Middleware
{
    // Bu middleware, gelen her isteği ve giden her yanıtı loglamak için kullanılır.
    // Yani, tüm istek ve yanıtların kaydedilmesini ve izlenmesini sağlar. ILogger arayüzünü kullanarak loglama yapar.
    //Bu sayede uygulamanızın çalışma zamanında ne tür isteklerin yapıldığını ve ne tür yanıtların döndüğünü takip edebilirsiniz.
    // Genellikle geliştirme ve hata ayıklama süreçlerinde kullanılır.
    //Global loglama yapan bir middleware(sadece actiona girildi gibi çok basit düzeyde)
    //app.log dosyasına yazdırır.

    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Request {context.Request.Method}: {context.Request.Path}");

            // Request'i sonraki middleware'lera iletmek için 
            await _next(context);

            // Response bilgisini de loglayabilmek için
            _logger.LogInformation($"Response {context.Response.StatusCode} for {context.Request.Method}: {context.Request.Path}");
        }
    }
    
}

