using PerfTips.Shared;
using PerfTips.Shared.Serializer;

namespace PerfTips.NodeClient.Commands;

public interface INodeCommand
{
    Task Execute(ITcpNode node, TcpMessage tcpMessage, ISerializer serializer, CancellationTokenSource cts);
}