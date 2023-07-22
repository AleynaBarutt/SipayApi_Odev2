using Microsoft.AspNetCore.Mvc;
using SipayApi.Attributes;
using SipayApi.Services;

namespace SipayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAuthenticationService _userService;

        public UserController(IUserAuthenticationService userService)
        {
            _userService = userService;
        }

        // FakeAuthorizeAttribute ile bu metodun giriş yapıldığında çalışmasını kontrol ederiz
        [HttpGet("profile")]
        [FakeAuthorize]
        public IActionResult GetUserProfile()
        {
            // Kullanıcının profiliyle ilgili işlemler burada yapılır
            // Örnek olarak giriş yapan kullanıcının adını döndürelim
             
            string username = HttpContext.User.Identity.Name;
            return Ok($"Welcome, {username}!");
        }
    }

}
