using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared;
using PerfTips.Shared.Enums;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class CleanNodeCommand : IServerCommand
{
    public async Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var nodeName = dataProvider.AskData("Node name to clean: ");

        var node = server.GetNodeInfo(nodeName);

        var message = new TcpMessage
        {
            Port = server.Port,
            Command = NodeCommands.CleanNode,
        };

        using var socket = await packageManager.SendPackage(message, new (server.IpAddress, node.Port));
    }
}