using PerfTips.Shared.Enums;
using ProtoBuf;

namespace PerfTips.Shared.MessageRecords;

[Serializable]
[ProtoContract]
public readonly record struct TcpMessage
{
    public int Port { get; init; }
    public NodeCommands Command { get; init; }
    public byte[] Data { get; init; }
}