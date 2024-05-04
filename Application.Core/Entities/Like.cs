using Application.Core.Entities.Base;

namespace Application.Core.Entities;

public class Like : BaseEntity
{
    public string UserId { get; set; }
    public Guid PostId { get; set; }
}
