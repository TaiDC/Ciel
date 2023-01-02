using System.IO;

using Ciel.WPF.Contracts.Services;
using Ciel.WPF.Core.Contracts.Services;
using Ciel.WPF.Core.Contracts.Services.API;
using Ciel.WPF.Core.Models;
using Ciel.WPF.Helpers;
using Ciel.WPF.Models;
using Ciel.WPF.ViewModels;

using Microsoft.Extensions.Options;

namespace Ciel.WPF.Services;

public class UserDataService : IUserDataService
{
    private readonly IFileService _fileService;
    private readonly IIdentityService _identityService;
    private readonly IAPIUserService _apiUserService;
    private readonly AppConfig _appConfig;
    private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private UserViewModel _user;

    public event EventHandler<UserViewModel> UserDataUpdated;

    public UserDataService(IFileService fileService, IIdentityService identityService, IAPIUserService apiUserService, IOptions<AppConfig> appConfig)
    {
        _fileService = fileService;
        _identityService = identityService;
        _apiUserService = apiUserService;
        _appConfig = appConfig.Value;
    }

    public void Initialize()
    {
        _identityService.LoggedIn += OnLoggedIn;
        _identityService.LoggedOut += OnLoggedOut;
    }

    public UserViewModel GetUser()
    {
        if (_user == null)
        {
            _user = GetUserFromCache();
            if (_user == null)
            {
                _user = GetDefaultUserData();
            }
        }

        return _user;
    }

    private async void OnLoggedIn(object sender, EventArgs e)
    {
        _user = await GetUserFromGraphApiAsync();
        UserDataUpdated?.Invoke(this, _user);
    }

    private void OnLoggedOut(object sender, EventArgs e)
    {
        _user = null;
        var folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);
        var fileName = _appConfig.UserFileName;
        _fileService.Save<User>(folderPath, fileName, null);
    }

    private UserViewModel GetUserFromCache()
    {
        var folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);
        var fileName = _appConfig.UserFileName;
        var cacheData = _fileService.Read<User>(folderPath, fileName);
        return GetUserViewModelFromData(cacheData);
    }

    private async Task<UserViewModel> GetUserFromGraphApiAsync()
    {
        var accessToken = await _identityService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            return null;
        }

        var userData = await _apiUserService.GetUserInfoAsync(accessToken);
        if (userData != null)
        {
            userData.Photo = await _apiUserService.GetUserPhoto(accessToken);
            var folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);
            var fileName = _appConfig.UserFileName;
            _fileService.Save<User>(folderPath, fileName, userData);
        }

        return GetUserViewModelFromData(userData);
    }

    private UserViewModel GetUserViewModelFromData(User userData)
    {
        if (userData == null)
        {
            return null;
        }

        var userPhoto = string.IsNullOrEmpty(userData.Photo)
            ? ImageHelper.ImageFromAssetsFile("DefaultIcon.png")
            : ImageHelper.ImageFromString(userData.Photo);

        return new UserViewModel()
        {
            Name = userData.DisplayName,
            UserName = userData.UserName,
            Photo = userPhoto
        };
    }

    private UserViewModel GetDefaultUserData()
    {
        return new UserViewModel()
        {
            Photo = ImageHelper.ImageFromAssetsFile("DefaultIcon.png")
        };
    }
}
