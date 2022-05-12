namespace PerfTips.Shared.MessageRecords;

[Serializable]
public class FileMessage
{
    public string PartialPath { get; set; }
    public byte[] FileData { get; set; }
}