namespace PerfTips.Shared.MessageRecords;

[Serializable]
public class AddFileMessage
{
    public AddFileMessage(string partialPath, byte[] fileData)
    {
        PartialPath = partialPath;
        FileData = fileData;
    }

    public string PartialPath { get; set; }
    public byte[] FileData { get; set; }
}