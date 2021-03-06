using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.Enums;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class RemoveFileCommand : IServerCommand
{
    public async Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var nodeName = dataProvider.AskData("Node name to add file: ");
        var fileFullPath = dataProvider.AskData($"File relative path on node {nodeName}: ");

        var node = server.GetNodeInfo(nodeName);

        var fileInfo = new FileInfo(fileFullPath);
        node.RemoveBytes(fileInfo.Length);

        FileMessage removeFileMessage = new () { PartialPath = fileInfo.FullName };

        var message = new TcpMessage
        {
            Command = NodeCommands.RemoveFile,
            Data = packageManager.Serializer.Serialize(removeFileMessage)
        };

        using var socket = await packageManager.SendPackage(message, new(server.IpAddress, node.Port));
    }
}