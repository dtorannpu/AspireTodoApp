var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();
var postgresdb = postgres.AddDatabase("postgresdb");

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireTodoApp_ApiService>("apiservice")
    .WithReference(postgresdb);

builder.AddProject<Projects.AspireTodoApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();