using AgentCoordinateWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<AgentChatCoordinator>();
    })
    .Build();

host.Run();
