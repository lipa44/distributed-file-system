using System.Net;
using AutoMapper;
using PerfTips.NodeClient.Commands;
using PerfTips.Shared;
using PerfTips.Shared.Enums;
using PerfTips.Shared.Serializer;

namespace PerfTips.NodeClient.TcpNode;

public class TcpNodeInstance : ITcpNode
{
    private readonly IMapper _mapper;
    private readonly ISerializer _serializer;
    private readonly List<FileInfo> _files = new();
    
    public TcpNodeInstance(string relativePath, IPAddress ip, int port, IMapper mapper, ISerializer serializer)
    {
        RelativePath = relativePath;
        IpAddress = ip;
        Port = port;
        _mapper = mapper;
        _serializer = serializer;
    }

    public string RelativePath { get; init; }
    public IPAddress IpAddress { get; init; }
    public int Port { get; init; }
    public IReadOnlyList<FileInfo> Files => _files;

    public async Task Execute(byte[] package, CancellationTokenSource cts)
    {
        var packageMessage = _serializer.Deserialize<TcpMessage>(package);

        var nodeCommand = _mapper.Map<ServerCommands, INodeCommand>(packageMessage.Command);

        await nodeCommand.Execute(this, packageMessage, _serializer, cts);

        cts.Token.ThrowIfCancellationRequested();
    }

    public async Task AddFile(FileInfo fileInfo, byte[] bytes)
    {
        if (IfFileExists(fileInfo)) throw new Exception("File to add to node already exists");

        _files.Add(fileInfo);
        
        fileInfo.Directory?.Create();
        await File.WriteAllBytesAsync(fileInfo.FullName, bytes);
    }

    public void RemoveFile(FileInfo fileInfo)
    {
        if (!IfFileExists(fileInfo)) throw new Exception("File to remove from node doesn't exist");

        File.Delete(fileInfo.FullName);
        
        _files.Remove(fileInfo);
    }
    
    private bool IfFileExists(FileInfo fileInfo) => _files.Any(n => n.Equals(fileInfo));
}
