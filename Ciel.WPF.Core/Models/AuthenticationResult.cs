namespace Ciel.WPF.Core.Models;

public class AuthenticationResult
{
    public string Token { get; set; }
    public string TokenType { get; set; }
    public DateTime ExpiresIn { get; set; }
}
