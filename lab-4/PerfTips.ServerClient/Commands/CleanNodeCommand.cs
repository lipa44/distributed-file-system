using System.Net;
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
        var nodeName = dataProvider.AskForData("Node name to clean: ");

        var node = serverInstance.GetNodeInfo(nodeName);

        var message = new TcpMessage
        {
            Port = serverInstance.Port,
            Command = ServerCommands.CleanNode,
        };

        var tcpEndPoint = new IPEndPoint(serverInstance.IpAddress, node.Port);
        packageManager.SendPackage(message, tcpEndPoint);

        return Task.CompletedTask;
    }
}