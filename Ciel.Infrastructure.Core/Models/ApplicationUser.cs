using Ciel.Infrastructure.Core.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Ciel.Infrastructure.Core.Models;

public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
{
    public ApplicationUser()
    {
        Roles = new HashSet<IdentityUserRole<string>>();
        Claims = new HashSet<IdentityUserClaim<string>>();
        Logins = new HashSet<IdentityUserLogin<string>>();
    }

    public string Photo { get; set; }

    // AuditInfo
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }

    // DeletableEntity
    public bool IsDeleted { get; set; }

    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
}