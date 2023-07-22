using SipayApi.Models;

namespace SipayApi.Extensions
{
    public static class StudentExtensions
    {
        //Api’nizde kullanılmak üzere extension geliştirin.
        public static string GetActivityStatus(this StudentResponse studentResponse)
        {
            return studentResponse.Age > 18 ? "Etkin" : "Etkin Değil";
        }
        public static string GetFullName(this StudentResponse studentResponse)
        {
            return $"{studentResponse.Name} {studentResponse.Lastname}";
        }
    }
}
