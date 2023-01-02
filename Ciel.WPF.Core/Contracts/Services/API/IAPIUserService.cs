using Ciel.WPF.Core.Models;

namespace Ciel.WPF.Core.Contracts.Services.API;

public interface IAPIUserService
{
    Task<User> GetUserInfoAsync(string accessToken);

    Task<string> GetUserPhoto(string accessToken);
}
