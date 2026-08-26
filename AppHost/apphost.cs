#:package Aspire.Hosting.PostgreSQL@13.5.3
#:package Aspire.Hosting.JavaScript@13.5.3
#:sdk Aspire.AppHost.Sdk@13.5.3
#:property AspireUseCliBundle=true
#:project ../api/HappiAdventure.Api.csproj

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithImage("postgis/postgis")
    .WithImageTag("16-3.4")
    .WithDataVolume();

var db = postgres.AddDatabase("Happi");

var api = builder.AddProject<Projects.HappiAdventure_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.AddViteApp("frontend", "../frontend")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
