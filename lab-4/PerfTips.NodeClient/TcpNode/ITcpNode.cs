using System.Net;
using System.Net.Sockets;

namespace PerfTips.NodeClient.TcpNode;

public interface ITcpNode
{
    Task Execute(Socket socket, CancellationTokenSource cts);

    Task AddFile(FileDescriptor fileDescriptor, byte[] bytes);
    void RemoveFile(FileDescriptor fileDescriptor);

    public string RelativePath { get; init; }
    public IPAddress IpAddress { get; init; }
    public int Port { get; init; }
    public IReadOnlyList<FileDescriptor> Files { get; }
}