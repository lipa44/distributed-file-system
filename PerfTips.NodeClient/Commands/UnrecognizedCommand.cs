using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared;
using PerfTips.Shared.Serializer;

namespace PerfTips.NodeClient.Commands;

public class UnrecognizedCommand : INodeCommand
{
    public Task Execute(ITcpNode node, TcpMessage tcpMessage, ISerializer serializer, CancellationTokenSource cts)
    {
        Console.WriteLine("Unrecognized command");

        return Task.CompletedTask;
    }
}