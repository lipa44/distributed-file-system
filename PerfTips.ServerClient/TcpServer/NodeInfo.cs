namespace PerfTips.ServerClient.TcpServer;

public class NodeInfo
{
    public NodeInfo(string name, int port, int maxSize)
    {
        Name = name;
        Port = port;
        MaxSize = maxSize;
        SizeAvailable = maxSize;
    }

    public string Name { get; }
    public int Port { get; }
    public long MaxSize { get; }
    public long SizeAvailable { get; private set; }

    public void AddBytes(long bytesAmount)
    {
        if (!IfEnoughSpaceToAdd(bytesAmount))
            throw new Exception("Not enough space to add file");

        SizeAvailable -= bytesAmount;
    }

    public void RemoveBytes(long bytesAmount) => SizeAvailable += bytesAmount;
    public void CleanNode() => SizeAvailable = MaxSize;
    public bool IfEnoughSpaceToAdd(long bytesAmount) => SizeAvailable > bytesAmount;

    public override string ToString() => $"{Name} <--> ...:{Port}";
    public override bool Equals(object? obj) => Equals(obj as NodeInfo);

    public override int GetHashCode() => HashCode.Combine(Name, Port);

    private bool Equals(NodeInfo? nodeInfo) => nodeInfo is not null && Name == nodeInfo.Name && Port == nodeInfo.Port;
}