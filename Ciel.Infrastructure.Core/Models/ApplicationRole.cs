using Ciel.Infrastructure.Core.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Ciel.Infrastructure.Core.Models;

public class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity
{
    public ApplicationRole(string name)
            : base(name)
    {
    }
    // AuditInfo
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }

    // DeletableEntity
    public bool IsDeleted { get; set; }
}