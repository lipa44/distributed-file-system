using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class ExecCommand : IServerCommand
{
    public async Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        throw new NotImplementedException();
    }
}