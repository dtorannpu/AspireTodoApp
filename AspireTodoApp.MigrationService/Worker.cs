using System.Diagnostics;
using AspireTodoApp.DataAccess.Contexts;
using AspireTodoApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;


namespace AspireTodoApp.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime
) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity(ActivityKind.Client);
        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();

            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(TodoContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () => { await dbContext.Database.MigrateAsync(cancellationToken); });
    }

    private static async Task SeedDataAsync(TodoContext dbContext, CancellationToken cancellationToken)
    {
        TodoItem item = new()
        {
            Name = "Test",
            IsComplete = true,
        };

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.TodoItems.AddAsync(item, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}