
using System.Buffers;
using System.Net.Sockets;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.NodeClient.Commands;

public class CleanNodeCommand : INodeCommand
{
    public async Task Execute(ITcpNode node, TcpMessage tcpMessage, Socket socket, IPackageManager packageManager, CancellationTokenSource cts)
    {
        // var filesToSend = new List<FileMessage>(node.Files.Count);

        // await packageManager.SendPackage(node.Files.Count, IPEndPoint.Parse($"{node.IpAddress}:{tcpMessage.Port}"));

        var fileSizeBytes = packageManager.Serializer.Serialize(node.Files.Count);
        await socket.SendAsync(fileSizeBytes);
        
        foreach (var fileDescriptor in node.Files)
        {
            var buffer = ArrayPool<byte>.Shared.Rent((int)fileDescriptor.FileInfo.Length);
            (await File.ReadAllBytesAsync(fileDescriptor.FileInfo.FullName)).CopyTo(buffer, 0);

            var fileMessage = new FileMessage
            {
                PartialPath = fileDescriptor.FilePath,
                FileData = buffer
            };

            await packageManager.SendFile(fileMessage, socket);
            // filesToSend.Add(fileMessage);
            ArrayPool<byte>.Shared.Return(buffer);
        }
        // await socket.SendAsync(packageManager.Serializer.Serialize(filesToSend));

        node.Clean();

        Console.WriteLine("Node cleaned");
    }
}