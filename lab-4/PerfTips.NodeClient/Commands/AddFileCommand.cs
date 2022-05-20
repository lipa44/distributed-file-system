using System.Net.Sockets;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.NodeClient.Commands;

public class AddFileCommand : INodeCommand
{
    public async Task Execute(ITcpNode node, TcpMessage tcpMessage, Socket socket, IPackageManager packageManager, CancellationTokenSource cts)
    {
        var addFileMessage = packageManager.Serializer.Deserialize<FileMessage>(tcpMessage.Data);

        var fileDescriptor = new FileDescriptor
        {
            FilePath = addFileMessage.PartialPath,
            FileInfo = new FileInfo(Path.Combine(node.RelativePath, $"{node.Port}", addFileMessage.PartialPath))
        };

        await node.AddFile(fileDescriptor, addFileMessage.FileData);
    }
}