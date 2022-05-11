namespace PerfTips.Shared.MessageRecords;

[Serializable]
public class RemoveFileMessage
{
    public RemoveFileMessage(string relativePath)
    {
        RelativePath = relativePath;
    }

    public string RelativePath { get; set; }
}