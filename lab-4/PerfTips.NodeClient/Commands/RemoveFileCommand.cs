using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.Serializer;

namespace PerfTips.NodeClient.Commands;

public class RemoveFileCommand : INodeCommand
{
    public Task Execute(ITcpNode node, TcpMessage tcpMessage, ISerializer serializer, CancellationTokenSource cts)
    {
        var addFileMessage = serializer.Deserialize<RemoveFileMessage>(tcpMessage.Data);

        var fileInfo = new FileInfo(Path.Combine(addFileMessage.FullPath));

        node.RemoveFile(fileInfo);

        return Task.CompletedTask;
    }
}