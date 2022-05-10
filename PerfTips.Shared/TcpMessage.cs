using PerfTips.Shared.Enums;

namespace PerfTips.Shared;

[Serializable]
public class TcpMessage
{
    public ServerCommands Command { get; init; }
    public byte[] Data { get; init; }
}