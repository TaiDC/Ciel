using Ciel.WPF.Contracts.Services;
using Ciel.WPF.Properties;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;

namespace Ciel.WPF.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IUserDataService _userDataService;

    [ObservableProperty] private HamburgerMenuItem _selectedMenuItem;
    [ObservableProperty] private HamburgerMenuItem _selectedOptionsMenuItem;

    // TODO: Change the icons and titles for all HamburgerMenuItems here.
    public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellHomePage, Glyph = "\ue80f", TargetPageType = typeof(HomeViewModel) },
    };

    public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellSettingsPage, Glyph = "\uE713", TargetPageType = typeof(SettingsViewModel) }
    };

    public ShellViewModel(INavigationService navigationService, IUserDataService userDataService)
    {
        _navigationService = navigationService;
        _userDataService = userDataService;
    }

    [RelayCommand]
    private void OnLoaded()
    {
        _navigationService.Navigated += OnNavigated;
        _userDataService.UserDataUpdated += OnUserDataUpdated;
        var user = _userDataService.GetUser();
        var userMenuItem = new HamburgerMenuImageItem()
        {
            Thumbnail = user.Photo,
            Label = user.Name,
            Command = UserItemSelectedCommand
        };

        OptionMenuItems.Insert(0, userMenuItem);
    }

    [RelayCommand]
    private void OnUnloaded()
    {
        _navigationService.Navigated -= OnNavigated;
        _userDataService.UserDataUpdated -= OnUserDataUpdated;
        var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
        if (userMenuItem != null)
        {
            OptionMenuItems.Remove(userMenuItem);
        }
    }

    private void OnUserDataUpdated(object sender, UserViewModel user)
    {
        var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
        if (userMenuItem != null)
        {
            userMenuItem.Label = user.Name;
            userMenuItem.Thumbnail = user.Photo;
        }
    }

    private bool CanGoBack()
        => _navigationService.CanGoBack;

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void OnGoBack()
        => _navigationService.GoBack();

    [RelayCommand]
    private void OnMenuItemInvoked()
        => NavigateTo(SelectedMenuItem.TargetPageType);

    [RelayCommand]
    private void OnOptionsMenuItemInvoked()
        => NavigateTo(SelectedOptionsMenuItem.TargetPageType);

    [RelayCommand]
    private void OnUserItemSelected()
        => NavigateTo(typeof(SettingsViewModel));

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel != null)
        {
            _navigationService.NavigateTo(targetViewModel.FullName);
        }
    }

    private void OnNavigated(object sender, string viewModelName)
    {
        var item = MenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        if (item != null)
        {
            SelectedMenuItem = item;
        }
        else
        {
            SelectedOptionsMenuItem = OptionMenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        }

        GoBackCommand.NotifyCanExecuteChanged();
    }
}
