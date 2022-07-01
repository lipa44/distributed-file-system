using PerfTips.Shared.Enums;

namespace PerfTips.Shared.MessageRecords;

[Serializable]
public readonly record struct TcpMessage(int Port, NodeCommands Command, byte[] Data);