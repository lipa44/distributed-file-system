namespace PerfTips.Shared.MessageRecords;

[Serializable]
public readonly record struct FileMessage
{
    public string PartialPath { get; init; }
    public byte[] FileData { get; init; }
}