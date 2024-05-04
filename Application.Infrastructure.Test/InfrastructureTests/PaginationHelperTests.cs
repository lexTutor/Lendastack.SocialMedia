using Application.Infrastructure.Helpers;

namespace Application.Infrastructure.Test.InfrastructureTests;

public class PaginationHelperTests
{
    private class Request
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    private class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Theory]
    [InlineData(10, 1, 0, 10)]
    [InlineData(10, 2, 10, 10)]
    [InlineData(10, 3, 20, 10)]
    [InlineData(10, 4, 30, 10)]
    [InlineData(10, 5, 40, 10)]
    public void SkipTake_Calculates_Correctly(int pageSize, int page, int expectedSkip, int expectedTake)
    {
        // Act
        var (actualSkip, actualTake) = PaginationHelper.SkipTake(pageSize, page);

        // Assert
        Assert.Equal(expectedSkip, actualSkip);
        Assert.Equal(expectedTake, actualTake);
    }

    [Theory]
    [InlineData(10, 10, 1)]
    [InlineData(15, 10, 2)]
    [InlineData(20, 10, 2)]
    [InlineData(25, 10, 3)]
    public void PageCount_Calculates_Correctly(long total, int take, long expectedPageCount)
    {
        // Act
        var actualPageCount = PaginationHelper.PageCount(total, take);

        // Assert
        Assert.Equal(expectedPageCount, actualPageCount);
    }

    [Fact]
    public void Paginate_Returns_Correct_Response()
    {
        // Arrange
        var data = Enumerable.Range(1, 100).Select(i => new Request { Id = i, Name = $"Item {i}" });
        IQueryable<Request> queryableData = data.AsQueryable();
        Func<IQueryable<Request>, IReadOnlyCollection<Response>> mapper = query => query.Select(r => new Response { Id = r.Id, Name = r.Name }).ToList();

        // Act
        var result = PaginationHelper.Paginate(queryableData, mapper, 2, 10, 100);

        // Assert
        Assert.Equal(2, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(100, result.TotalCount);
        Assert.Equal(10, result.PageCount);
        Assert.Equal(data.Skip(10).Take(10).Select(s => $"{s.Name}"), result.Data.Select(s => $"{s.Name}"));
    }
}
