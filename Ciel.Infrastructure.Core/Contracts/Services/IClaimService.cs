namespace Ciel.Infrastructure.Core.Contracts.Services;

public interface IClaimService
{
    public string? GetUserId();
    public string? GetClaim(string key);
}