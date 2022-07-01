using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class GetNodesCommand : IServerCommand
{
    public Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        for (var i = 0; i < server.Nodes.Count; ++i)
            Console.WriteLine($"{i + 1}) {server.Nodes[i]}");

        return Task.CompletedTask;
    }
}