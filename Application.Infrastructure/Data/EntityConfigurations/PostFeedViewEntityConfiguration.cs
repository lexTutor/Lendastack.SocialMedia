using Application.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Infrastructure.Data.EntityConfigurations
{
    public class PostFeedViewEntityConfiguration : IEntityTypeConfiguration<PostFeedView>
    {
        public void Configure(EntityTypeBuilder<PostFeedView> builder)
        {
            builder.Property(x => x.FollowerId).IsRequired(false);
            builder.ToView(nameof(PostFeedView)).HasNoKey();
        }
    }
}
