namespace PerfTips.ServerClient.Helpers;

public record AppSettings
{
    public string Path { get; init; }
    public int Port { get; set; }
    public int BufferSize { get; set; }
    public string Server { get; set; }
}