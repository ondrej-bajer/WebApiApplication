namespace WebApiApplication.DTOs.Auth
{
    public sealed record LoginResponse(
        string AccessToken,
        string TokenType,
        DateTime ExpiresUtc
    );
}
