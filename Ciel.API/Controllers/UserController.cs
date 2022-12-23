using Ciel.API.Core;
using Ciel.API.Core.CommandHandlers.User;
using Ciel.API.Core.QueryHandlers.User;
using Ciel.Infrastructure.Core.Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ciel.API.Controllers;

public class UserController : APIController
{
    private readonly IMediator _mediator;
    private readonly IClaimService _claimService;
    public UserController(IMediator mediator, IClaimService claimService)
    {
        _mediator = mediator;
        _claimService = claimService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(CreateUserRequest createUserModel)
    {
        return Ok(ApiResult<CreateUserResponse>.Success(await _mediator.Send(createUserModel)));
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        var userId = _claimService.GetUserId() ?? throw new ArgumentNullException();

        var getUserInfoRequest = new GetUserInfoByIdRequest(userId);

        return Ok(ApiResult<GetUserInfoByIdResponse>.Success(await _mediator.Send(getUserInfoRequest)));
    }

    [HttpGet("photo")]
    [Authorize]
    public async Task<IActionResult> GetUserPhotoAsync()
    {
        var userId = _claimService.GetUserId() ?? throw new ArgumentNullException();

        var getPhotoByIdRequest = new GetPhotoByIdRequest(userId);

        return Ok(ApiResult<string>.Success(await _mediator.Send(getPhotoByIdRequest)));
    }
}