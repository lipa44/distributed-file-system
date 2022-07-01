namespace PerfTips.NodeClient.Helpers;

public readonly record struct AppSettings
{
    public int BufferSize { get; init; }
    public string Server { get; init; }
    public string RelativePath { get; init; }
}