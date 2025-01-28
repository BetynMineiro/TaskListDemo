using Task.CrossCutting.Enum.Common;

namespace Task.Domain.Entities.Base;

public abstract class EntityBase
{
    public DateTimeOffset CreatedAt { get;  set; }
    public DateTimeOffset? UpdatedAt { get;  set; }
    public Status Status { get; set; } = Status.Active;

    public void SetCreationInfo()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void SetUpdateInfo()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}