using System.Net;
using AutoMapper;
using PerfTips.ServerClient.Commands;
using PerfTips.ServerClient.DataProviders;
using PerfTips.Shared.Enums;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.TcpServer;

public class Server : IServer
{
    private readonly IMapper _mapper;
    private readonly IDataProvider _provider;
    private readonly IPackageManager _packageManager;
    private readonly List<NodeInfo> _nodes = new ();

    public Server(IPAddress ipAddress, int port, IMapper mapper, IDataProvider provider, IPackageManager packageManager)
    {
        IpAddress = ipAddress;
        Port = port;
        _mapper = mapper;
        _provider = provider;
        _packageManager = packageManager;

        Console.WriteLine($"PerfTips.ServerClient started on IP: {IpAddress.MapToIPv4()} (v4) | {IpAddress.MapToIPv6()} (v6)");
    }

    public IPAddress IpAddress { get; init; }
    public int Port { get; }
    public IReadOnlyList<NodeInfo> Nodes => _nodes;

    public async Task Execute(ServerCommands command, CancellationTokenSource cts)
    {
        var serverCommand = _mapper.Map<ServerCommands, IServerCommand>(command);

        await serverCommand.Execute(this, _packageManager, _provider, cts);
    }

    public void AddNode(NodeInfo node)
    {
        if (IfNodeExists(node)) throw new Exception($"Node {node} already exists");

        _nodes.Add(node);
    }

    public void RemoveNode(NodeInfo node)
    {
        if (!IfNodeExists(node)) throw new Exception($"Node {node} was not exist");

        _nodes.Remove(node);
    }

    public NodeInfo GetNodeInfo(string nodeName) => _nodes.Single(n => n.Name == nodeName);
    
    private bool IfNodeExists(NodeInfo tcpNode) => _nodes.Any(n => n.Equals(tcpNode));
}