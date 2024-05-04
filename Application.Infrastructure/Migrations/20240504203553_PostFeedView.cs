using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Infrastructure.Migrations
{
    public partial class PostFeedView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = $@"
                CREATE VIEW {nameof(PostFeedView)} AS
                SELECT p.Id as PostId, ur.FollowerId, u.LastName, u.FirstName, p.UserId, p.Text, p.CreatedOn, p.UpdatedOn, COUNT(l.PostId) AS Likes
                FROM Post p
                INNER JOIN AspNetUsers u ON p.UserId = u.Id
                LEFT JOIN UserRelationship ur ON p.UserId = ur.FollowedId
                LEFT JOIN [Like] l ON p.Id = l.PostId
                GROUP BY p.Id, ur.FollowerId, u.LastName, u.FirstName, p.UserId, p.Text, p.CreatedOn, p.UpdatedOn
                ";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"DROP VIEW {nameof(PostFeedView)}");
        }
    }
}
