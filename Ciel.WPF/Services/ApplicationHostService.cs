using Ciel.WPF.Contracts.Activation;
using Ciel.WPF.Contracts.Services;
using Ciel.WPF.Contracts.Views;
using Ciel.WPF.Core.Contracts.Services;
using Ciel.WPF.Helpers;
using Ciel.WPF.Models;
using Ciel.WPF.ViewModels;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Ciel.WPF.Services;

public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly IPersistAndRestoreService _persistAndRestoreService;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IIdentityService _identityService;
    private readonly IUserDataService _userDataService;
    private readonly AppConfig _appConfig;

    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private IShellWindow _shellWindow;
    private ILogInWindow _logInWindow;
    private bool _isInitialized;

    public ApplicationHostService(IServiceProvider serviceProvider, IEnumerable<IActivationHandler> activationHandlers, INavigationService navigationService, IThemeSelectorService themeSelectorService, IPersistAndRestoreService persistAndRestoreService, IIdentityService identityService, IUserDataService userDataService, IOptions<AppConfig> config)
    {
        _serviceProvider = serviceProvider;
        _activationHandlers = activationHandlers;
        _navigationService = navigationService;
        _themeSelectorService = themeSelectorService;
        _persistAndRestoreService = persistAndRestoreService;
        _identityService = identityService;
        _userDataService = userDataService;
        _appConfig = config.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Initialize services that you need before app activation
        await InitializeAsync();

        if (!_isInitialized)
        {

        }

        if (!_identityService.IsLoggedIn())
        {
            if (!_isInitialized)
            {
                _logInWindow = _serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
                _logInWindow.ShowWindow();
                await StartupAsync();
                _isInitialized = true;
            }

            return;
        }

        await HandleActivationAsync();

        // Tasks after activation
        await StartupAsync();
        _isInitialized = true;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _persistAndRestoreService.PersistData();
        await Task.CompletedTask;
        _identityService.LoggedIn -= OnLoggedIn;
        _identityService.LoggedOut -= OnLoggedOut;
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _persistAndRestoreService.RestoreData();
            _themeSelectorService.InitializeTheme();
            _userDataService.Initialize();
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            await Task.CompletedTask;
        }
    }

    private async Task StartupAsync()
    {
        if (!_isInitialized)
        {
            await Task.CompletedTask;
        }
    }

    private async Task HandleActivationAsync()
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle());

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync();
        }

        await Task.CompletedTask;

        if (App.Current.Windows.OfType<IShellWindow>().Count() == 0)
        {
            // Default activation that navigates to the apps default page
            _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(HomeViewModel).FullName);
            await Task.CompletedTask;
        }
    }

    private async void OnLoggedIn(object sender, EventArgs e)
    {
        await HandleActivationAsync();
        _logInWindow.CloseWindow();
    }

    private void OnLoggedOut(object sender, EventArgs e)
    {
        _logInWindow = _serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
        _logInWindow.ShowWindow();

        _shellWindow.CloseWindow();
        _navigationService.UnsubscribeNavigation();
    }
}
