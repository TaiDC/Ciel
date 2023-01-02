using Ciel.WPF.Core.Models;

namespace Ciel.WPF.Core.Contracts.Services.API;

public interface IAPIIdentityService
{
    public Task<AuthenticationResult> LoginAsync(string username, string password);
}
