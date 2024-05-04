using Application.Core.Entities.Base;

namespace Application.Core.Entities;

public class Post : BaseEntity
{
    public string Text { get; set; }
    public string UserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
    public virtual IList<Like> Likes { get; set; }
}
