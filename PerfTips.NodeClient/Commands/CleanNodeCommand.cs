using PerfTips.Shared;
using PerfTips.Shared.Serializer;

namespace PerfTips.NodeClient.Commands;

public class CleanNodeCommand : INodeCommand
{
    public Task Execute(ITcpNode node, TcpMessage tcpMessage, ISerializer serializer, CancellationTokenSource cts)
    {
        foreach (var file in new List<FileInfo>(node.Files))
            node.RemoveFile(file);

        return Task.CompletedTask;
    }
}