using System.Windows.Input;

using Ciel.WPF.Contracts.Services;
using Ciel.WPF.Contracts.ViewModels;
using Ciel.WPF.Core.Contracts.Services;
using Ciel.WPF.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Options;

namespace Ciel.WPF.ViewModels;

// TODO: Change the URL for your privacy policy in the appsettings.json file, currently set to https://YourPrivacyUrlGoesHere
public partial class SettingsViewModel : ObservableObject, INavigationAware
{
    private readonly AppConfig _appConfig;
    private readonly IUserDataService _userDataService;
    private readonly IIdentityService _identityService;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly ISystemService _systemService;
    private readonly IApplicationInfoService _applicationInfoService;

    [ObservableProperty] private AppTheme _theme;
    [ObservableProperty] private string _versionDescription;
    [ObservableProperty] private UserViewModel _user;

    public SettingsViewModel(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService, ISystemService systemService, IApplicationInfoService applicationInfoService, IUserDataService userDataService, IIdentityService identityService)
    {
        _appConfig = appConfig.Value;
        _themeSelectorService = themeSelectorService;
        _systemService = systemService;
        _applicationInfoService = applicationInfoService;
        _userDataService = userDataService;
        _identityService = identityService;
    }

    public void OnNavigatedTo(object parameter)
    {
        VersionDescription = $"{Properties.Resources.AppDisplayName} - {_applicationInfoService.GetVersion()}";
        Theme = _themeSelectorService.GetCurrentTheme();
        _identityService.LoggedOut += OnLoggedOut;
        _userDataService.UserDataUpdated += OnUserDataUpdated;
        User = _userDataService.GetUser();
    }

    public void OnNavigatedFrom()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        _identityService.LoggedOut -= OnLoggedOut;
        _userDataService.UserDataUpdated -= OnUserDataUpdated;
    }

    [RelayCommand]
    private void OnSetTheme(string themeName)
    {
        var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
        _themeSelectorService.SetTheme(theme);
    }

    [RelayCommand]
    private void OnPrivacyStatement()
        => _systemService.OpenInWebBrowser("https://github.com/TaiDC");

    [RelayCommand]
    private async void OnLogOut()
    {
        await _identityService.LogoutAsync();
    }

    private void OnUserDataUpdated(object sender, UserViewModel userData)
    {
        User = userData;
    }

    private void OnLoggedOut(object sender, EventArgs e)
    {
        UnregisterEvents();
    }
}
