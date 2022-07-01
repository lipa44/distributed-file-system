using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class QuitCommand : IServerCommand
{
    public Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource cts)
    {
        cts.Cancel();

        return Task.CompletedTask;
    }
}