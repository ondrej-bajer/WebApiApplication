using System.Security.Claims;
using WebApiApplication.DTOs.Auth;

namespace WebApiApplication.Interfaces
{
    public interface ITokenService
    {
        LoginResponse CreateToken(string username, string role);
        IEnumerable<Claim> BuildClaims(string username, string role);
    }
}
