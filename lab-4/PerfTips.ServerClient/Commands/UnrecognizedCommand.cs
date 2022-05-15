using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class UnrecognizedCommand : IServerCommand
{
    public Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        Console.WriteLine("Unrecognized command");

        return Task.CompletedTask;
    }
}