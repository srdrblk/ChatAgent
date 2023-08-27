using ShiftControlWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ShiftControl>();
    })
    .Build();

host.Run();
