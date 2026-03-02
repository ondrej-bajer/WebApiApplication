using WebApiApplication.DTOs.Auth;

namespace WebApiApplication.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Validates demo credentials and returns token response if OK, otherwise null.
        /// </summary>
        LoginResponse? Login(LoginRequest request);
    }
}
