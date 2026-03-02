using Microsoft.Extensions.Options;
using WebApiApplication.Configuration;
using WebApiApplication.DTOs.Auth;
using WebApiApplication.Interfaces;

namespace WebApiApplication.Services.Security
{
    public sealed class AuthService : IAuthService
    {
        private readonly DemoUsersOptions _users;
        private readonly ITokenService _tokenService;

        public AuthService(IOptions<DemoUsersOptions> usersOptions, ITokenService tokenService)
        {
            _users = usersOptions.Value;
            _tokenService = tokenService;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var user = _users.Users.FirstOrDefault(u =>
                string.Equals(u.Username, request.Username, StringComparison.OrdinalIgnoreCase)
                && u.Password == request.Password);

            if (user is null)
                return null;

            return _tokenService.CreateToken(user.Username, user.Role);
        }
    }
}
