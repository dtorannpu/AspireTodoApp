using AspireTodoApp.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AspireTodoApp.Tests.UnitTests.Helpers;

internal class MockDb : IDbContextFactory<TodoContext>
{
    public TodoContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new TodoContext(options);
    }
}