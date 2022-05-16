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
        var filesToSend = new List<FileMessage>();

        foreach (var fileDescriptor in new List<FileDescriptor>(node.Files))
        {
            var fileBytes = await File.ReadAllBytesAsync(fileDescriptor.FileInfo.FullName);

            filesToSend.Add(new FileMessage
            {
                PartialPath = fileDescriptor.FilePath,
                FileData = fileBytes
            });
        }

        node.Clean();

        var message = new TcpMessage
        {
            Data = packageManager.Serializer.Serialize(filesToSend)
        };

        await socket.SendAsync(packageManager.Serializer.Serialize(message));

        Console.WriteLine("Node cleaned");
    }
}