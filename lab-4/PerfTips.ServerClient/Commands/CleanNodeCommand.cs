using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared;
using PerfTips.Shared.Enums;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class CleanNodeCommand : IServerCommand
{
    public Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var nodeName = dataProvider.AskData("Node name to clean: ");

        var node = serverInstance.GetNodeInfo(nodeName);

        var message = new TcpMessage
        {
            Port = serverInstance.Port,
            Command = NodeCommands.CleanNode,
        };

        packageManager.SendPackage(message, new (serverInstance.IpAddress, node.Port));

        return Task.CompletedTask;
    }
}