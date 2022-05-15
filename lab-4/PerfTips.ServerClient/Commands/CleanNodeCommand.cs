using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared;
using PerfTips.Shared.Enums;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class CleanNodeCommand : IServerCommand
{
    public Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var nodeName = dataProvider.AskData("Node name to clean: ");

        var node = server.GetNodeInfo(nodeName);

        var message = new TcpMessage
        {
            Port = server.Port,
            Command = NodeCommands.CleanNode,
        };

        packageManager.SendPackage(message, new (server.IpAddress, node.Port));

        return Task.CompletedTask;
    }
}