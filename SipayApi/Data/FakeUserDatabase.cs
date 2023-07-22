using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
namespace SipayApi.Data
{
    public class FakeUserDatabase
    {
       // FakeUserDatabase sınıfı, e-posta ve şifre bilgilerini içeren bir sözlük kullanarak kullanıcı kimlik doğrulama
       // işlemini sağlar.ValidateUser metodu, verilen e-posta ve şifreyi sözlükteki verilerle karşılaştırır
       // ve doğrulama başarılı ise kullanıcının kimlik numarasını döndürür.Diğer ValidateUser metodu ise kullanılmamaktadır ve
       // sadece NotImplementedException fırlatır.

        private static Dictionary<string, (string Password, int UserId)> users = new Dictionary<string, (string, int)>
        {
            {"aleyna@gmail.com", ("123", 1)},
            {"ebru@gmail.com", ("456", 2)},
        };
        public static bool ValidateUser(string email, string password, out int userId)
        {
            userId = 0;
            if (users.TryGetValue(email, out var userData))
            {
                if (password == userData.Password)
                {
                    userId = userData.UserId;
                    return true;
                }
            }
            return false;
        }

        internal static bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
