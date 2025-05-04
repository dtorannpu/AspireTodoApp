using AspireTodoApp.DataAccess.Contexts;
using AspireTodoApp.MigrationService;


var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));
builder.AddNpgsqlDbContext<TodoContext>("postgresdb");
var host = builder.Build();
host.Run();