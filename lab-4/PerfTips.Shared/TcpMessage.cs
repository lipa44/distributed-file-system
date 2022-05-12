using PerfTips.Shared.Enums;

namespace PerfTips.Shared;

[Serializable]
public class TcpMessage
{
    public int Port { get; init; }
    public NodeCommands Command { get; init; }
    public byte[] Data { get; init; }
}