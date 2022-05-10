using System.Net;
using PerfTips.Shared.Enums;

namespace PerfTips.ServerClient;

public interface IServerInstance
{
    public IPAddress IpAddress { get; init; }
    Task Execute(ServerCommands command, CancellationTokenSource cts);
    void AddNode(NodeInfo node);
    void RemoveNode(NodeInfo node);
}