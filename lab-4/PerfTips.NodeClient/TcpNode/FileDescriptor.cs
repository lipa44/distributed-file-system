using ProtoBuf;

namespace PerfTips.NodeClient.TcpNode;

[Serializable]
[ProtoContract]
public readonly record struct FileDescriptor
{
    [ProtoMember(1)]
    public string FilePath { get; init; }
    [ProtoMember(2)]
    public FileInfo FileInfo { get; init; }
}