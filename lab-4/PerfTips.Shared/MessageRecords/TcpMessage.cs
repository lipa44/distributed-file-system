using PerfTips.Shared.Enums;
using ProtoBuf;

namespace PerfTips.Shared.MessageRecords;

[ProtoContract]
public readonly record struct TcpMessage
{
    [ProtoMember(1)]
    public int Port { get; init; }
    [ProtoMember(2)]
    public NodeCommands Command { get; init; }
    [ProtoMember(3)]
    public byte[] Data { get; init; }
}