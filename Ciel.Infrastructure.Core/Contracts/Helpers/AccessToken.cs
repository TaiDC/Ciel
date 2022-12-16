namespace Ciel.Infrastructure.Core.Contracts.Helpers;

public class AccessToken
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public int RefreshTokenExpiresIn { get; set; }
}