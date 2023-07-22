using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SipayApi.Services;

namespace SipayApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class FakeAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // IHttpContextAccessor ile kullanıcının bilgilerine erişim
            var httpContext = context.HttpContext;
            var userService = httpContext.RequestServices.GetService<IUserAuthenticationService>();

            // Kullanıcı girişi kontrolü
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Kullanıcı adı ve parola alınır
            string username = httpContext.User.Identity.Name;
            string password = httpContext.Request.Headers["X-Password"].ToString(); // Örnek olarak X-Password header'ını kullanıyoruz

            // Kullanıcı adı ve parola doğrulama işlemi
            if (!userService.ValidateUser(username, password))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
    
}
