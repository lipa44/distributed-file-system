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
    public async Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var filePath = dataProvider.AskData("File path (absolute): ");
        var nodeName = dataProvider.AskData("Node name to add file: ");
        var fileRelativePath = dataProvider.AskData($"File relative path on node {nodeName}: ");

        var node = server.GetNodeInfo(nodeName);

        var fileInfo = new FileInfo(filePath);
        node.AddBytes(fileInfo.Length);

        var bytes = await File.ReadAllBytesAsync(filePath);

        FileMessage fileMessage = new FileMessage
        {
            PartialPath = Path.Combine(nodeName, fileRelativePath, fileInfo.Name),
            FileData = bytes
        };

        var message = new TcpMessage
        {
            Command = NodeCommands.AddFile,
            Data = packageManager.Serializer.Serialize(fileMessage)
        };

        var socket = packageManager.SendPackage(message, new (server.IpAddress, node.Port));

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}