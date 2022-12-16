using Ciel.Infrastructure.Core.Contracts.Helpers;
using Ciel.Infrastructure.Core.Models;

namespace Ciel.Infrastructure.Core.Contracts.Services;

public interface IJwtService
{
    Task<AccessToken> GetAccessTokenAsync(ApplicationUser user);
    int? ValidateAccessTokenAsync(string token);
}