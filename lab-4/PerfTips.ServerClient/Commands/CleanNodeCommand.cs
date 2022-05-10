using System.Net;
using System.Net.Sockets;
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
            Command = ServerCommands.CleanNode,
        };

        var tcpEndPoint = new IPEndPoint(serverInstance.IpAddress, node.Port);
        var socket = packageManager.SendPackage(message, tcpEndPoint);

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();

        return Task.CompletedTask;
    }
}