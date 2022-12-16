using Ciel.Infrastructure.Core.Contracts.Helpers;
using Ciel.Infrastructure.Core.Contracts.Services;
using Ciel.Infrastructure.Core.Models;
using Ciel.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Ciel.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public JwtService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<AccessToken> GetAccessTokenAsync(ApplicationUser user)
    {
        user = user ?? throw new NullReferenceException(nameof(user));

        var claims = await GetClaimsAsync(user);

        var tokenBuilder = new TokenBuilder()
            .AddAudience(_configuration["JWT:Audience"])
            .AddIssuer(_configuration["JWT:Issuer"])
            .AddExpiryDate(1)
            .AddKey(_configuration["JWT:Key"])
            .AddClaims(claims)
            .Build();

        return GenerateAccessToken(tokenBuilder);
    }

    public int? ValidateAccessTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        authClaims.AddRange(userClaims);

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        return authClaims;
    }


    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    private AccessToken GenerateAccessToken(JwtSecurityToken securityToken)
    {
        securityToken = securityToken ?? throw new ArgumentNullException(nameof(securityToken));
        return new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            TokenType = "Bearer",
            ExpiresIn = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds
        };
    }
}