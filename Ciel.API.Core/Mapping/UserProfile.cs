using AutoMapper;
using Ciel.API.Core.CommandHandlers.User;
using Ciel.API.Core.QueryHandlers.User;
using Ciel.Infrastructure.Core.Models;

namespace Ciel.API.Core.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequest, ApplicationUser>();
        CreateMap<ApplicationUser, GetUserInfoByIdResponse>();
    }
}
