using ProtoBuf;

namespace PerfTips.NodeClient.TcpNode;

[Serializable]
[ProtoContract]
public readonly record struct FileDescriptor
{
    public string FilePath { get; init; }
    public FileInfo FileInfo { get; init; }
}