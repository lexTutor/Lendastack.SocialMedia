namespace Application.Core.Entities;

public class PostFeedView
{
    public Guid PostId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName => $"{FirstName} {LastName}";
    public int Likes { get; set; }
    public string Text { get; set; }
    public string UserId { get; set; }
    public string FollowerId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
