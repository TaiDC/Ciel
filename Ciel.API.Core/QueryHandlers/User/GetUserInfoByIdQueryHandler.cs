using AutoMapper;
using Ciel.Infrastructure.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ciel.API.Core.QueryHandlers.User;

public class GetUserInfoByIdRequest : IRequest<GetUserInfoByIdResponse>
{
    public string Id { get; set; }

    public GetUserInfoByIdRequest(string id)
    {
        Id = id;
    }
}

public class GetUserInfoByIdResponse
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Mail { get; set; }

    public string DisplayName { get; set; }

    public string GivenName { get; set; }

    public string Surname { get; set; }

    public object JobTitle { get; set; }

    public string MobilePhone { get; set; }

    public object OfficeLocation { get; set; }

    public string PreferredLanguage { get; set; }

    public string Photo { get; set; }
}

public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdRequest, GetUserInfoByIdResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    public GetUserInfoByIdQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<GetUserInfoByIdResponse> Handle(GetUserInfoByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        return _mapper.Map<GetUserInfoByIdResponse>(user);
    }
}