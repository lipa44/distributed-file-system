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
    public async Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var files = new List<FileMessage>();
        var nodes = server.Nodes;

        foreach (var node in nodes)
        {
            var message = new TcpMessage
            {
                Port = server.Port,
                Command = NodeCommands.CleanNode,
            };

            using var socket = packageManager.SendPackage(message, new (server.IpAddress, node.Port));

            var package = packageManager.ReceivePackage(socket);

            try
            {
                var receivedMessage = packageManager.Serializer.Deserialize<TcpMessage>(package);
                var receivedFiles = packageManager.Serializer.Deserialize<List<FileMessage>>(receivedMessage.Data);
                files.AddRange(receivedFiles);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            node.CleanNode();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        foreach (var file in files.OrderByDescending(f => f.FileData.Length))
        {
            var node = FindLessLoadedNode(nodes);
            Console.WriteLine($"Less loaded node: {node.Name}; File size: {file.FileData.Length}");
            node.AddBytes(file.FileData.Length);

            FileMessage fileMessage = new FileMessage
            {
                PartialPath = Path.Combine(node.Name, file.PartialPath),
                FileData = file.FileData
            };

            var message = new TcpMessage
            {
                Command = NodeCommands.AddFile,
                Data = packageManager.Serializer.Serialize(fileMessage)
            };

            using var socket = packageManager.SendPackage(message, new (server.IpAddress, node.Port));
        }
    }

    private NodeInfo FindLessLoadedNode(IReadOnlyList<NodeInfo> nodes) => nodes.MaxBy(f => (double) f.SizeAvailable / f.MaxSize);
}