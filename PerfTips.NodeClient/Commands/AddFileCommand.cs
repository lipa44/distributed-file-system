using PerfTips.Shared;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.Serializer;

namespace PerfTips.NodeClient.Commands;

public class AddFileCommand : INodeCommand
{
    public async Task Execute(ITcpNode node, TcpMessage tcpMessage, ISerializer serializer, CancellationTokenSource cts)
    {
        var addFileMessage = serializer.Deserialize<AddFileMessage>(tcpMessage.Data);

        var fileInfo = new FileInfo(Path.Combine(node.RelativePath, addFileMessage.PartialPath));

        await node.AddFile(fileInfo, addFileMessage.FileData);
    }
}