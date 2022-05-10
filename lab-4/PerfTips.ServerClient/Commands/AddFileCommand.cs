using System.Net;
using System.Net.Sockets;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared;
using PerfTips.Shared.Enums;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class AddFileCommand : IServerCommand
{
    public async Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        // /Users/lipa/Desktop/test.md

        var filePath = dataProvider.AskForData("File path (absolute): ");
        var nodeName = dataProvider.AskForData("Node name to add file: ");
        var fileRelativePath = dataProvider.AskForData($"File relative path on node {nodeName}: ");

        var node = serverInstance.GetNodeInfo(nodeName);

        var fileInfo = new FileInfo(filePath);

        var bytes = await File.ReadAllBytesAsync(filePath);
        AddFileMessage addFileMessage = new (Path.Combine(nodeName, fileRelativePath, fileInfo.Name), bytes);

        var message = new TcpMessage
        {
            Command = ServerCommands.AddFile,
            Data = packageManager.Serializer.Serialize(addFileMessage)
        };

        var tcpEndPoint = new IPEndPoint(serverInstance.IpAddress, node.Port);
        var socket = packageManager.SendPackage(message, tcpEndPoint);

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}