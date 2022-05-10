using PerfTips.ServerClient.DataProviders;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class UnrecognizedCommand : IServerCommand
{
    public Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        Console.WriteLine("Unrecognized command");

        return Task.CompletedTask;
    }
}