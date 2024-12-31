var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NetEngCore403>("NetEngCore403");

builder.Build().Run();
