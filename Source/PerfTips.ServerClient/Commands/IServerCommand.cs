using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public interface IServerCommand
{
    Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource cts);
}