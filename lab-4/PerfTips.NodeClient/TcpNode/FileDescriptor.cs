namespace PerfTips.NodeClient.TcpNode;

public readonly record struct FileDescriptor
{
    public string FilePath { get; init; }
    public FileInfo FileInfo { get; init; }
}