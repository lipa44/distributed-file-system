namespace PerfTips.ServerClient;

public class NodeInfo
{
    public NodeInfo(string name, int port, int maxSize)
    {
        Name = name;
        Port = port;
        MaxSize = maxSize;
    }

    public string Name { get; init; }
    public int Port { get; init; }
    public int MaxSize { get; init; }

    public override string ToString() => $"{Name} <--> ...:{Port}";
    public override bool Equals(object? obj) => Equals(obj as NodeInfo);

    public override int GetHashCode() => HashCode.Combine(Name, Port);

    private bool Equals(NodeInfo? nodeInfo) => nodeInfo is not null && Name == nodeInfo.Name && Port == nodeInfo.Port;
}