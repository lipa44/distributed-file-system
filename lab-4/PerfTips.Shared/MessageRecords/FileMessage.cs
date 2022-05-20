using ProtoBuf;

namespace PerfTips.Shared.MessageRecords;

[Serializable]
[ProtoContract]
public readonly record struct FileMessage
{
    [ProtoMember(1)]
    public string PartialPath { get; init; }
    [ProtoMember(2)]
    public byte[] FileData { get; init; }
}