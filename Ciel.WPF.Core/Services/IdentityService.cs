using Ciel.WPF.Core.Contracts.Services;
using Ciel.WPF.Core.Contracts.Services.API;
using Ciel.WPF.Core.Helpers;
using Ciel.WPF.Core.Models;
using System.Net.NetworkInformation;

namespace Ciel.WPF.Core.Services;

public class IdentityService : IIdentityService
{
    private readonly IAPIIdentityService _apiIdentityService;

    private AuthenticationResult _authenticationResult;

    public event EventHandler LoggedIn;

    public event EventHandler LoggedOut;

    public IdentityService(IAPIIdentityService apiIdentityService)
    {
        _apiIdentityService = apiIdentityService;
    }
    public bool IsLoggedIn() => _authenticationResult != null;

    public async Task<LoginResultType> LoginAsync(string username, string password)
    {
        if (!NetworkInterface.GetIsNetworkAvailable())
        {
            return LoginResultType.NoNetworkAvailable;
        }

        try
        {
            _authenticationResult = await _apiIdentityService.LoginAsync(username, password);
            if (!IsAuthorized())
            {
                _authenticationResult = null;
                return LoginResultType.Unauthorized;
            }

            LoggedIn?.Invoke(this, EventArgs.Empty);
            return LoginResultType.Success;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            return LoginResultType.UnknownError;
        }
    }

    public bool IsAuthorized()
    {
        // TODO: You can also add extra authorization checks here.
        // i.e.: Checks permisions of _authenticationResult.Account.Username in a database.
        return true;
    }

    public string GetAccountUserName()
    {
        //TODO: Bo sung sau
        return null;
    }

    public async Task LogoutAsync()
    {
        try
        {
            _authenticationResult = null;
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception)
        {
            // TODO: LogoutAsync can fail please handle exceptions as appropriate to your scenario
            // For more info on MsalExceptions see
            // https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/exceptions
        }
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var acquireTokenSuccess = await AcquireTokenSilentAsync();
        if (acquireTokenSuccess)
        {
            return _authenticationResult.Token;
        }
        else
        {
            try
            {
                // Interactive authentication is required
                //var accounts = await _client.GetAccountsAsync();
                //_authenticationResult = await _client.AcquireTokenInteractive(scopes)
                //                                     .WithAccount(accounts.FirstOrDefault())
                //                                     .ExecuteAsync();
                return _authenticationResult.Token;
            }
            catch (Exception)
            {
                // AcquireTokenSilent and AcquireTokenInteractive failed, the session will be closed.
                _authenticationResult = null;
                LoggedOut?.Invoke(this, EventArgs.Empty);
                return string.Empty;
            }
        }
    }

    public async Task<bool> AcquireTokenSilentAsync()
    {
        if (!NetworkInterface.GetIsNetworkAvailable())
        {
            return false;
        }

        try
        {
            //var accounts = await _client.GetAccountsAsync();
            //_authenticationResult = await _client.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
            //                                     .ExecuteAsync();
            return true;
        }
        catch (Exception)
        {
            // TODO: Silentauth failed, please handle this exception as appropriate to your scenario

            return false;
        }
    }

    private void ConfigureCache()
    {
        //if (_identityCacheService != null)
        //{
        //    // .NET core applications should provide a mechanism to serialize and deserialize the user token cache
        //    // https://aka.ms/msal-net-token-cache-serialization
        //    _client.UserTokenCache.SetBeforeAccess((args) =>
        //    {
        //        var data = _identityCacheService.ReadMsalToken();
        //        if (data != default)
        //        {
        //            args.TokenCache.DeserializeMsalV3(data);
        //        }
        //    });
        //    _client.UserTokenCache.SetAfterAccess((args) =>
        //    {
        //        if (args.HasStateChanged)
        //        {
        //            _identityCacheService.SaveMsalToken(args.TokenCache.SerializeMsalV3());
        //        }
        //    });
        //}
    }
}
