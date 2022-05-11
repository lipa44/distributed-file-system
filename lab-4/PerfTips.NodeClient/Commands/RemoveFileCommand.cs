using System.Net.Sockets;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.NodeClient.Commands;

public class RemoveFileCommand : INodeCommand
{
    public Task Execute(ITcpNode node, TcpMessage tcpMessage, Socket socket, IPackageManager packageManager, CancellationTokenSource cts)
    {
        var addFileMessage = packageManager.Serializer.Deserialize<RemoveFileMessage>(tcpMessage.Data);

        var fileDescriptor = new FileDescriptor
        {
            FilePath = addFileMessage.RelativePath,
            FileInfo = new FileInfo(Path.Combine(node.RelativePath, addFileMessage.RelativePath))
        };

        node.RemoveFile(fileDescriptor);

        return Task.CompletedTask;
    }
}