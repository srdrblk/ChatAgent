using MonitorService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<MonitorService.Monitor>();
    })
    .Build();

host.Run();
