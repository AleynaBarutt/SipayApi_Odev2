namespace SipayApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        //Tüm istisnaları yakalayarak özel bir işlem gerçekleştirir. Örnek olarak, istisnaları loglayabilir ve kullanıcıya uygun
        // bir hata mesajı döndürebilir. Bu sayede uygulamanınçalışma zamanında meydana gelen istisnaları ele alabilir
        // ve kullanıcıların daha iyi bir deneyim yaşamasını sağlayabiliriz.
        // Genellikle hata yönetimi ve kullanıcıya hata mesajları gönderme işlemlerinde kullanılır.
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // İstisnayı ILogger arayüzü ile logla
                _logger.LogError(ex, $"An exception occurred: {ex.Message}");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occurred. Please try again later.");
            }
        }
    }
}
