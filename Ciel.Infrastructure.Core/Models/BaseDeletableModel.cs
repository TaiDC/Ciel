using Ciel.Infrastructure.Core.Contracts;

namespace Ciel.Infrastructure.Core.Models;

public class BaseDeletableMode : BaseModel, IDeletableEntity
{
    public bool IsDeleted { get; set; }
}