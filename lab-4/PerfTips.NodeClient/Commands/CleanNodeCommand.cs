using System.Net.Sockets;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.PackageManager;

namespace PerfTips.NodeClient.Commands;

public class CleanNodeCommand : INodeCommand
{
    public async Task Execute(ITcpNode node, TcpMessage tcpMessage, Socket socket, IPackageManager packageManager, CancellationTokenSource cts)
    {
        var filesToSend = new List<FileMessage>(node.Files.Count);

        foreach (var fileDescriptor in new List<FileDescriptor>(node.Files))
        {
            var fileBytes = await File.ReadAllBytesAsync(fileDescriptor.FileInfo.FullName);

            var fileMessage = new FileMessage
            {
                PartialPath = fileDescriptor.FilePath,
                FileData = fileBytes
            };

            filesToSend.Add(fileMessage);
        }

        await socket.SendAsync(packageManager.Serializer.Serialize(filesToSend));

        node.Clean();

        Console.WriteLine("Node cleaned");
    }
}