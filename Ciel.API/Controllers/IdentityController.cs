using Ciel.API.Core;
using Ciel.API.Core.CommandHandlers.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ciel.API.Controllers;

public class IdentityController : APIController
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginRequest loginRequestModel)
    {
        return Ok(ApiResult<LoginResponse>.Success(await _mediator.Send(loginRequestModel)));
    }
}