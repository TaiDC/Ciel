using Ciel.Infrastructure.Core.Contracts;

namespace Ciel.Infrastructure.Core.Models;

public class BaseModel : IEntity<int>, IAuditInfo
{
    public int Id { get; }

    // AuditInfo
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }

}