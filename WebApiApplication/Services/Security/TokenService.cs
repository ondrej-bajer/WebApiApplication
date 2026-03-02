using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiApplication.Configuration;
using WebApiApplication.DTOs.Auth;
using WebApiApplication.Interfaces;

namespace WebApiApplication.Services.Security

{
    public sealed class TokenService : ITokenService
    {
        private readonly JwtOptions _jwt;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwt = jwtOptions.Value;
        }

        public LoginResponse CreateToken(string username, string role)
        {
            var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiresMinutes);

            var claims = BuildClaims(username, role);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse(tokenString, "Bearer", expires);
        }

        public IEnumerable<Claim> BuildClaims(string username, string role)
        {
            return new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        }
    }
}
