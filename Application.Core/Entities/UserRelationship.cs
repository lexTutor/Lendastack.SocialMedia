using Application.Core.Entities.Base;

namespace Application.Core.Entities;

public class UserRelationship : BaseEntity
{
    public string FollowerId { get; set; }
    public string FollowedId { get; set; }
}
