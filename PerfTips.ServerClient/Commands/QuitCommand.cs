using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class QuitCommand : IServerCommand
{
    public Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource cts)
    {
        cts.Cancel();

        return Task.CompletedTask;
    }
}