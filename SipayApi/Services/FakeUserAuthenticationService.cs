using SipayApi.Data;

namespace SipayApi.Services
{
    public class FakeUserAuthenticationService : IUserAuthenticationService
    {
        public bool ValidateUser(string username, string password)
        {
            // Kullanıcı adı ve parolayı FakeUserDatabase'den kontrol ediyoruz
            return FakeUserDatabase.ValidateUser(username, password);
        }
    }

}
