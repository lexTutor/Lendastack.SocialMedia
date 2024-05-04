using Application.Core.Entities;
using Application.Infrastructure.Data.Repository;
using Application.Services.Services.LikePost;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

namespace Application.Infrastructure.Test.ServiceTests;

public class LikePostCommandHandlerTests
{
    private readonly LikePostCommandHandler _handler;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IRepository<Like>> _likeRepositoryMock;
    private readonly Mock<IRepository<Post>> _postRepositoryMock;
    private static string UserId = Guid.NewGuid().ToString();

    public LikePostCommandHandlerTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserId)
            }, "TestAuth"))),
        });

        _likeRepositoryMock = new Mock<IRepository<Like>>();
        _postRepositoryMock = new Mock<IRepository<Post>>();

        _handler = new LikePostCommandHandler(
            _httpContextAccessorMock.Object,
            _likeRepositoryMock.Object,
            _postRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_Success()
    {
        // Arrange
        var command = new LikePostCommand { PostId = Guid.NewGuid() };
        var post = new Post { Id = command.PostId };

        _postRepositoryMock.Setup(x => x.GetNotTrackedSingle(
                It.IsAny<Expression<Func<Post, bool>>>(),
                It.IsAny<Func<IQueryable<Post>, IOrderedQueryable<Post>>>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Post, object>>[]>()))
                .ReturnsAsync(post);

        var likes = new List<Like>().AsQueryable().BuildMock();

        _likeRepositoryMock.Setup(x => x.GetNonTrackedDbSet(
             It.IsAny<Expression<Func<Like, bool>>>(),
             It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(),
             It.IsAny<bool>(),
             It.IsAny<Expression<Func<Like, object>>[]>()))
            .Returns(likes);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task Handle_PostNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = new LikePostCommand { PostId = Guid.NewGuid() };

        _postRepositoryMock.Setup(x => x.GetNotTrackedSingle(
             It.IsAny<Expression<Func<Post, bool>>>(), null, false, null))
             .Returns<Post>(null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task Handle_PostAlreadyLiked_ReturnsConflict()
    {
        // Arrange
        var command = new LikePostCommand { PostId = Guid.NewGuid() };
        var post = new Post { Id = command.PostId };

        _postRepositoryMock.Setup(x => x.GetNotTrackedSingle(
            It.IsAny<Expression<Func<Post, bool>>>(), null, false, null))
            .Returns<Post>(null);

        var likes = new List<Like>
        {
            new()
            {
                PostId = command.PostId,
                UserId = UserId,
                CreatedOn = DateTime.UtcNow
            }
        }.AsQueryable().BuildMock();

        _likeRepositoryMock.Setup(x => x.GetNonTrackedDbSet(
             It.IsAny<Expression<Func<Like, bool>>>(),
             It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(),
             It.IsAny<bool>(),
             It.IsAny<Expression<Func<Like, object>>[]>()))
            .Returns(likes);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.False(result.Succeeded);
    }
}
