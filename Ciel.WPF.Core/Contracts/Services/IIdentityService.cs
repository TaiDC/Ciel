using Ciel.WPF.Core.Helpers;

namespace Ciel.WPF.Core.Contracts.Services;

public interface IIdentityService
{
    event EventHandler LoggedIn;

    event EventHandler LoggedOut;

    bool IsLoggedIn();

    Task<LoginResultType> LoginAsync(string username, string password);

    bool IsAuthorized();

    string GetAccountUserName();

    Task LogoutAsync();

    Task<string> GetAccessTokenAsync();

    Task<bool> AcquireTokenSilentAsync();
}
