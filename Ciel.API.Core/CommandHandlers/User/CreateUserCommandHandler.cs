using AutoMapper;
using Ciel.Infrastructure.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ciel.API.Core.CommandHandlers.User;
public class CreateUserRequest : IRequest<CreateUserResponse>
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class CreateUserResponse
{
    public string Id { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            throw new Exception($"Username '{request.UserName}' is already taken.");
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail != null)
        {
            throw new Exception($"Email '{request.Email}' is already registered.");
        }

        var user = _mapper.Map<ApplicationUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new Exception();
        }

        return new CreateUserResponse()
        {
            Id = user.Id
        };
    }
}