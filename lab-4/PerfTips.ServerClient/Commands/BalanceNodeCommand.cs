using System.Net;
using System.Net.Sockets;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared;
using PerfTips.Shared.Enums;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class BalanceNodeCommand : IServerCommand
{
    public async Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var files = new List<FileMessage>();
        var nodes = serverInstance.Nodes;

        foreach (var node in nodes)
        {
            var message = new TcpMessage
            {
                Port = serverInstance.Port,
                Command = ServerCommands.CleanNode,
            };

            var tcpEndPoint = new IPEndPoint(serverInstance.IpAddress, node.Port);
            var socket = packageManager.SendPackage(message, tcpEndPoint);

            var package = packageManager.ReceivePackage(socket);

            var receivedMessage = packageManager.Serializer.Deserialize<TcpMessage>(package);
            var receivedFiles = packageManager.Serializer.Deserialize<List<FileMessage>>(receivedMessage.Data);
            files.AddRange(receivedFiles);

            node.CleanNode();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        foreach (var file in files.OrderByDescending(f => f.FileData.Length))
        {
            var node = FindLessLoadedNode(nodes);
            Console.WriteLine($"Less loaded node: {node.Name}; File size: {file.FileData.Length}");
            node.AddBytes(file.FileData.Length);

            FileMessage fileMessage = new (Path.Combine(node.Name, file.PartialPath), file.FileData);

            var message = new TcpMessage
            {
                Command = ServerCommands.AddFile,
                Data = packageManager.Serializer.Serialize(fileMessage)
            };

            var tcpEndPoint = new IPEndPoint(serverInstance.IpAddress, node.Port);
            packageManager.SendPackage(message, tcpEndPoint);
        }
    }

    private NodeInfo FindLessLoadedNode(IReadOnlyList<NodeInfo> nodes) => nodes.MaxBy(f => (double) f.SizeAvailable / f.MaxSize);
}