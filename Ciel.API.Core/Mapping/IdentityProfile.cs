using AutoMapper;
using Ciel.API.Core.CommandHandlers.Identity;
using Ciel.Infrastructure.Core.Contracts.Helpers;

namespace Ciel.API.Core.Mapping;

public class IdentityProfile : Profile
{
	public IdentityProfile()
	{
		CreateMap<AccessToken, LoginResponse>();
	}
}