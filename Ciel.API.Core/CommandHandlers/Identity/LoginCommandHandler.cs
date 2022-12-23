using AutoMapper;
using Ciel.Infrastructure.Core.Contracts.Services;
using Ciel.Infrastructure.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ciel.API.Core.CommandHandlers.Identity;

public class LoginRequest : IRequest<LoginResponse>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
}
public class LoginCommandHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IJwtService _jwtService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    public LoginCommandHandler(IJwtService jwtService, SignInManager<ApplicationUser> signInManager, IMapper mapper)
    {
        _jwtService = jwtService;
        _signInManager = signInManager;
        _userManager = signInManager.UserManager;
        _mapper = mapper;
    }

    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            throw new Exception();
        }

        var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            throw new Exception();
        }

        return _mapper.Map<LoginResponse>(await _jwtService.GetAccessTokenAsync(user));
    }
}