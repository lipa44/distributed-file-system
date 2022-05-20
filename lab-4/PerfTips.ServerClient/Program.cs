using System.Net;
using AutoMapper;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;
using PerfTips.ServerClient;

if (args.Any() && bool.TryParse(args[0], out bool isWithFilesGeneration) && isWithFilesGeneration)
    PerfTips.FilesGenerator.Program.Main();

var appSettings = Startup.AppSettings;

IDataProvider commandsProvider = Startup.DataProvider;
IPackageManager packageManager = Startup.PackageManager;
IMapper mapper = Startup.Mapper;

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (s, e) =>
{
    cts.Cancel();
    e.Cancel = true;
};

try
{
    Server server = new(IPAddress.Parse(appSettings.Server), appSettings.Port, mapper, commandsProvider, packageManager);

    while (!cts.IsCancellationRequested)
    {
        var command = commandsProvider.GetCommand();
        await server.Execute(command, cts);
    }

    cts.Token.ThrowIfCancellationRequested();
}
catch (OperationCanceledException)
{
    Console.WriteLine("Cancellation requested, program stopped");
    Console.ReadLine();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.ReadLine();
}