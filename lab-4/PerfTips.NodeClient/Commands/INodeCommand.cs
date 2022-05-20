using System.Net.Sockets;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.NodeClient.Commands;

public interface INodeCommand
{
    Task Execute(ITcpNode node, TcpMessage tcpMessage, Socket socket, IPackageManager packageManager, CancellationTokenSource cts);
}