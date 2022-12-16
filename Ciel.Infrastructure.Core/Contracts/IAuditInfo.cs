namespace Ciel.Infrastructure.Core.Contracts;

public interface IAuditInfo
{
    string? CreatedBy { get; set; }

    DateTime CreatedOn { get; set; }

    string? UpdatedBy { get; set; }

    DateTime UpdatedOn { get; set; }
}