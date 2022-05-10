using System.Net;
using System.Net.Sockets;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared;
using PerfTips.Shared.Enums;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;
using PerfTips.Shared.Serializer;

namespace PerfTips.ServerClient.Commands;

public class RemoveFileCommand : IServerCommand
{
    public Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var nodeName = dataProvider.AskForData("Node name to add file: ");
        var fileFullPath = dataProvider.AskForData($"File full path on node {nodeName}: ");

        var node = serverInstance.GetNodeInfo(nodeName);

        RemoveFileMessage removeFileMessage = new (Path.Combine(fileFullPath));

        var message = new TcpMessage
        {
            Command = ServerCommands.RemoveFile,
            Data = packageManager.Serializer.Serialize(removeFileMessage)
        };

        var tcpEndPoint = new IPEndPoint(serverInstance.IpAddress, node.Port);
        var socket = packageManager.SendPackage(message, tcpEndPoint);

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();

        return Task.CompletedTask;
    }
}