namespace SipayApi.Services
{
    public interface IUserAuthenticationService
    {
        bool ValidateUser(string username, string password);
    }
}
