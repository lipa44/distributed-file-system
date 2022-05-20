using System.Buffers;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
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

        var buffer = ArrayPool<byte>.Shared.Rent((int)fileInfo.Length);
        (await File.ReadAllBytesAsync(filePath)).CopyTo(buffer, 0);

        FileMessage fileMessage = new FileMessage
        {
            PartialPath = Path.Combine(fileRelativePath, fileInfo.Name),
            FileData = buffer
        };

        ArrayPool<byte>.Shared.Return(buffer);

        var message = new TcpMessage
        {
            Command = NodeCommands.AddFile,
            Data = packageManager.Serializer.Serialize(fileMessage)
        };

        using var socket = await packageManager.SendPackage(message, new (server.IpAddress, node.Port));
    }
}