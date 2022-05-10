using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class BalanceNodeCommand : IServerCommand
{
    public Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        throw new NotImplementedException();
    }
}