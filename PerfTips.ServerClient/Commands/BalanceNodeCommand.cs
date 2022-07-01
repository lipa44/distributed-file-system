using System.Net.Sockets;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.Enums;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class BalanceNodeCommand : IServerCommand
{
    public async Task Execute(Server server, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        var files = new List<FileMessage>(server.Nodes.Count);
        var nodes = server.Nodes;

        foreach (var node in nodes)
        {
            var message = new TcpMessage
            {
                Port = server.Port,
                Command = NodeCommands.CleanNode,
            };

            using var socket = await packageManager.SendPackage(message, new (server.IpAddress, node.Port));

            var filesCountBuffer = new byte[sizeof(int)];
            await socket.ReceiveAsync(filesCountBuffer);
            var filesCount = packageManager.Serializer.Deserialize<int>(filesCountBuffer);

            for (int i = 0; i < filesCount; i++)
            {
                try
                {
                    var file = await packageManager.ReceiveFile(socket);
                    files.Add(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            node.CleanNode();
        }

        foreach (var file in files.OrderByDescending(f => f.FileData.Length))
        {
            var node = FindLessLoadedNode(nodes);
            Console.WriteLine($"{node.Name}: SizeAvailable: {node.SizeAvailable}; {file.PartialPath}: {file.FileData.Length};");
            node.AddBytes(file.FileData.Length);

            FileMessage fileMessage = new FileMessage
            {
                PartialPath = file.PartialPath,
                FileData = file.FileData
            };

            var message = new TcpMessage
            {
                Command = NodeCommands.AddFile,
                Data = packageManager.Serializer.Serialize(fileMessage)
            };

            using var socket = await packageManager.SendPackage(message, new (server.IpAddress, node.Port));

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }

    private NodeInfo FindLessLoadedNode(IReadOnlyList<NodeInfo> nodes) => nodes.MaxBy(f => (double) f.SizeAvailable / f.MaxSize);
}