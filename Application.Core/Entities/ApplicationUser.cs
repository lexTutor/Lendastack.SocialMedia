using Microsoft.AspNetCore.Identity;

namespace Application.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual IList<Like> Likes { get; set; }
        public virtual IList<Post> Posts { get; set; }
    }
}
