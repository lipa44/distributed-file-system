using System.Net.Sockets;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared;
using PerfTips.Shared.PackageManager;

namespace PerfTips.NodeClient.Commands;

public class UnrecognizedCommand : INodeCommand
{
    public Task Execute(ITcpNode node, TcpMessage tcpMessage, Socket socket, IPackageManager packageManager, CancellationTokenSource cts)
    {
        Console.WriteLine("Unrecognized command");

        return Task.CompletedTask;
    }
}