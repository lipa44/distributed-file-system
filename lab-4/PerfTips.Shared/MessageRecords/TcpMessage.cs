using PerfTips.Shared.Enums;

namespace PerfTips.Shared.MessageRecords;

[Serializable]
public readonly record struct TcpMessage
{
    public int Port { get; init; }
    public NodeCommands Command { get; init; }
    public byte[] Data { get; init; }
}