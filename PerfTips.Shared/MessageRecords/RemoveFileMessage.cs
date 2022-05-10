namespace PerfTips.Shared.MessageRecords;

[Serializable]
public class RemoveFileMessage
{
    public RemoveFileMessage(string fullPath)
    {
        FullPath = fullPath;
    }

    public string FullPath { get; set; }
}