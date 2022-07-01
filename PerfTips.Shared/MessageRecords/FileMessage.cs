namespace PerfTips.Shared.MessageRecords;

[Serializable]
public readonly record struct FileMessage(string PartialPath, byte[] FileData);