using AspireTodoApp.ApiService.Controllers;
using AspireTodoApp.ApiService.Data;
using AspireTodoApp.ApiService.Dtos;
using AspireTodoApp.ApiService.Models;
using AspireTodoApp.Tests.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace AspireTodoApp.Tests.UnitTests.Controllers;
public class TodoItemsControllerTests
{
    [Fact]
    public async Task GetTodoItems_ReturnsEmptyList_WhenNoItemsExist()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        var controller = new TodoItemsController(context);

        // Act
        var result = await controller.GetTodoItems();

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(okResult.Value);
        Assert.Empty(items);
    }

    [Fact]
    public async Task GetTodoItems_ReturnsItems_WhenItemsExist()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        context.TodoItems.Add(new TodoItem { Id = 1, Name = "Test Item 1", IsComplete = false });
        context.TodoItems.Add(new TodoItem { Id = 2, Name = "Test Item 2", IsComplete = true });
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);

        // Act
        var result = await controller.GetTodoItems();

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(okResult.Value);
        Assert.Equal(2, items.Count());
        Assert.Contains(items, i => i.Name == "Test Item 1");
        Assert.Contains(items, i => i.Name == "Test Item 2");
    }

    [Fact]
    public async Task GetTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        var controller = new TodoItemsController(context);

        // Act
        var result = await controller.GetTodoItem(999); // 存在しないIDを指定

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetTodoItem_ReturnsItem_WhenItemExists()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        var todoItem = new TodoItem { Id = 1, Name = "Test Item", IsComplete = true };
        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);

        // Act
        var result = await controller.GetTodoItem(1); // 存在するIDを指定

        // Assert
        var okResult = Assert.IsType<ActionResult<TodoItemDTO>>(result);
        var item = Assert.IsType<TodoItemDTO>(okResult.Value);
        Assert.Equal(1, item.Id);
        Assert.Equal("Test Item", item.Name);
        Assert.True(item.IsComplete);
    }

    [Fact]
    public async Task PutTodoItem_UpdatesItem_WhenItemExists()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        var todoItem = new TodoItem { Id = 1, Name = "Old Name", IsComplete = false };
        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);
        var updatedTodo = new TodoItemDTO { Id = 1, Name = "Updated Name", IsComplete = true };

        // Act
        var result = await controller.PutTodoItem(1, updatedTodo);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedItem = await context.TodoItems.FindAsync(1L);
        Assert.NotNull(updatedItem);
        Assert.Equal("Updated Name", updatedItem.Name);
        Assert.True(updatedItem.IsComplete);
    }

    [Fact]
    public async Task PutTodoItem_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        var controller = new TodoItemsController(context);
        var updatedTodo = new TodoItemDTO { Id = 2, Name = "Updated Name", IsComplete = true };

        // Act
        var result = await controller.PutTodoItem(1, updatedTodo);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task PutTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        var controller = new TodoItemsController(context);
        var updatedTodo = new TodoItemDTO { Id = 1, Name = "Updated Name", IsComplete = true };

        // Act
        var result = await controller.PutTodoItem(1, updatedTodo);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PostTodoItem_CreatesNewItem()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        var controller = new TodoItemsController(context);
        var newTodo = new TodoItemDTO { Name = "New Item", IsComplete = false };

        // Act
        var result = await controller.PostTodoItem(newTodo);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdItem = Assert.IsType<TodoItemDTO>(createdResult.Value);
        Assert.Equal("New Item", createdItem.Name);
        Assert.False(createdItem.IsComplete);

        var itemInDb = await context.TodoItems.FindAsync(createdItem.Id);
        Assert.NotNull(itemInDb);
        Assert.Equal("New Item", itemInDb.Name);
        Assert.False(itemInDb.IsComplete);
    }

    [Fact]
    public async Task DeleteTodoItem_RemovesItem_WhenItemExists()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        var todoItem = new TodoItem { Id = 1, Name = "Test Item", IsComplete = false };
        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);

        // Act
        var result = await controller.DeleteTodoItem(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var deletedItem = await context.TodoItems.FindAsync(1L);
        Assert.Null(deletedItem);
    }

    [Fact]
    public async Task DeleteTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        var controller = new TodoItemsController(context);

        // Act
        var result = await controller.DeleteTodoItem(999); // 存在しないID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PutTodoItem_ReturnsNotFound_WhenSaveChangesThrowsDbUpdateConcurrencyException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        await using var context = new TodoContext(options);
        var todoItem = new TodoItem { Id = 2, Name = "Test Item", IsComplete = false };
        var data = new List<TodoItem>
        {
            new TodoItem { Id = 1, Name = "Test Item 1", IsComplete = false },
        }.AsQueryable();

        var mockSet = new Mock<DbSet<TodoItem>>();
        mockSet.As<IQueryable<TodoItem>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<TodoItem>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<TodoItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(todoItem);

        // モックの DbContext を作成
        var mockContext = new Mock<TodoContext>(options);
        mockContext.Setup(m => m.TodoItems)
            .Returns(mockSet.Object);
        mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateConcurrencyException());

        var controller = new TodoItemsController(mockContext.Object);
        var updatedTodo = new TodoItemDTO { Id = 2, Name = "Updated Name", IsComplete = true };

        // Act
        var result = await controller.PutTodoItem(2, updatedTodo);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PutTodoItem_ReturnsNotFound_WhenSaveChangesFails()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        await using var context = new TodoContext(options);
        var todoItem = new TodoItem { Id = 1, Name = "Test Item", IsComplete = false };
        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        // モックの DbContext を作成
        var mockContext = new Mock<TodoContext>(options);
        mockContext.Setup(m => m.TodoItems.FindAsync(1L))
            .ReturnsAsync(todoItem);
        mockContext.Setup(m => m.SaveChangesAsync(default))
            .ThrowsAsync(new DbUpdateConcurrencyException());

        var controller = new TodoItemsController(mockContext.Object);
        var updatedTodo = new TodoItemDTO { Id = 1, Name = "Updated Name", IsComplete = true };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
        {
            await controller.PutTodoItem(1, updatedTodo);
        });
    }
}