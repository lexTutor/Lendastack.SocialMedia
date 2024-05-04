using Application.Core.Entities;
using Application.Core.Entities.Base;
using Application.Infrastructure.Data.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Infrastructure.Data.DbContext;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> getData) : base(getData)
    { }

    public DbSet<Post> Post { get; set; }
    public DbSet<Like> Like { get; set; }
    public DbSet<PostFeedView> PostFeedView { get; set; }
    public DbSet<UserRelationship> UserRelationship { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var item in ChangeTracker.Entries<BaseEntity>())
        {
            switch (item.State)
            {
                case EntityState.Modified:
                    item.Entity.UpdatedOn = DateTime.UtcNow;
                    break;
                case EntityState.Added:
                    item.Entity.CreatedOn = DateTime.UtcNow;
                    break;
                default:
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PostEntityConfiguration());
        builder.ApplyConfiguration(new FollowerEntityConfiguration());
        builder.ApplyConfiguration(new LikeEntityConfiguration());
        builder.ApplyConfiguration(new PostFeedViewEntityConfiguration());

        base.OnModelCreating(builder);
    }
}
