using PerfTips.ServerClient.DataProviders;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public interface IServerCommand
{
    Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource cts);
}