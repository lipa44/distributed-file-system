using System.Net;
using AutoMapper;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient;

public static class Program
{
    public static async Task Main()
    {
        var appSettings = Startup.AppSettings;

        IDataProvider commandsProvider = Startup.DataProvider;
        IPackageManager packageManager = Startup.PackageManager;
        IMapper mapper = Startup.Mapper;

        var cts = new CancellationTokenSource();
        
        Console.CancelKeyPress += (s, e) =>
        {
            Console.WriteLine("Canceling...");
            cts.Cancel();
            e.Cancel = true;
        };

        try
        {
            Server server = new (IPAddress.Parse(appSettings.Server), appSettings.Port, mapper, commandsProvider, packageManager);

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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }
}