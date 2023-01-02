using Ciel.WPF.Contracts.Services;
using Ciel.WPF.Core.Contracts.Services;
using Ciel.WPF.Core.Helpers;
using Ciel.WPF.Properties;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ciel.WPF.ViewModels;

public partial class LogInViewModel : ObservableObject
{
    private readonly IIdentityService _identityService;

    [ObservableProperty] private string _statusMessage;

    [ObservableProperty] private string _userName;

    [ObservableProperty] private string _password;

    [ObservableProperty] private UserViewModel _user;

    public LogInViewModel(IIdentityService identityService, IUserDataService userDataService)
    {
        _identityService = identityService;

        User = userDataService.GetUser();
        UserName = User.UserName;
    }

    [RelayCommand]
    private async Task Login()
    {
        StatusMessage = string.Empty;
        var loginResult = await _identityService.LoginAsync(UserName, Password);
        StatusMessage = GetStatusMessage(loginResult);
    }

    private string GetStatusMessage(LoginResultType loginResult)
    {
        return loginResult switch
        {
            LoginResultType.Unauthorized => Resources.StatusUnauthorized,
            LoginResultType.NoNetworkAvailable => Resources.StatusNoNetworkAvailable,
            LoginResultType.UnknownError => Resources.StatusLoginFails,
            LoginResultType.Success or LoginResultType.CancelledByUser => string.Empty,
            _ => string.Empty,
        };
    }
}
