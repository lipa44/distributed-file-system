using System.Net;

namespace PerfTips.NodeClient.TcpNode;

public interface ITcpNode
{
    Task Execute(byte[] package, CancellationTokenSource cts);

    Task AddFile(FileInfo fileInfo, byte[] bytes);
    void RemoveFile(FileInfo fileInfo);

    public string RelativePath { get; init; }
    public IPAddress IpAddress { get; init; }
    public int Port { get; init; }
    public IReadOnlyList<FileInfo> Files { get; }
}