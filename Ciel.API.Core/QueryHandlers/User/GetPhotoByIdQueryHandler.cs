using Ciel.API.Core.Helpers;
using Ciel.Infrastructure.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ciel.API.Core.QueryHandlers.User;

public class GetPhotoByIdRequest : IRequest<string>
{
    public string Id { get; set; }

    public GetPhotoByIdRequest(string id)
    {
        Id = id;
    }
}

public class GetPhotoByIdQueryHandler : IRequestHandler<GetPhotoByIdRequest, string>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetPhotoByIdQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(GetPhotoByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        
        //TODO: Replace SecurityStamp to PhotoPath
        if (user != null && user.SecurityStamp != null && File.Exists(user.SecurityStamp))
        {
            using var stream = File.OpenWrite(user.SecurityStamp);
            return stream.ToBase64String();
        }
        return string.Empty;
    }
}
