namespace PerfTips.NodeClient.Helpers;

public record AppSettings
{
    public int BufferSize { get; set; }
    public string Server { get; set; }
    public string RelativePath { get; set; }
}