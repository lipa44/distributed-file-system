using System.Net;
using System.Net.Sockets;
using AutoMapper;
using PerfTips.NodeClient.Commands;
using PerfTips.Shared;
using PerfTips.Shared.Enums;
using PerfTips.Shared.PackageManager;

namespace PerfTips.NodeClient.TcpNode;

public class TcpNodeInstance : ITcpNode
{
    private readonly IMapper _mapper;
    private readonly IPackageManager _packageManager;
    private readonly List<FileDescriptor> _files = new();
    
    public TcpNodeInstance(string relativePath, IPAddress ip, int port, IMapper mapper, IPackageManager packageManager)
    {
        RelativePath = relativePath;
        IpAddress = ip;
        Port = port;
        _mapper = mapper;
        _packageManager = packageManager;
    }

    public string RelativePath { get; init; }
    public IPAddress IpAddress { get; init; }
    public int Port { get; init; }
    public IReadOnlyList<FileDescriptor> Files => _files;

    public async Task Execute(Socket socket, byte[] package, CancellationTokenSource cts)
    {
        var packageMessage = _packageManager.Serializer.Deserialize<TcpMessage>(package);

        var nodeCommand = _mapper.Map<ServerCommands, INodeCommand>(packageMessage.Command);

        await nodeCommand.Execute(this, packageMessage, socket, _packageManager, cts);

        cts.Token.ThrowIfCancellationRequested();
    }

    public async Task AddFile(FileDescriptor fileDescriptor, byte[] bytes)
    {
        if (IfFileExists(fileDescriptor)) throw new Exception("File to add to node already exists");

        _files.Add(fileDescriptor);

        var fileInfo = new FileInfo(fileDescriptor.FileInfo.FullName);

        fileInfo.Directory?.Create();
        await File.WriteAllBytesAsync(fileInfo.FullName, bytes);

        Console.WriteLine($"File {fileInfo} added");
    }

    public void RemoveFile(FileDescriptor fileDescriptor)
    {
        if (!IfFileExists(fileDescriptor)) throw new Exception("File to remove from node doesn't exist");

        File.Delete(fileDescriptor.FileInfo.FullName);

        _files.Remove(fileDescriptor);

        Console.WriteLine($"File {fileDescriptor.FilePath} removed");
    }
    
    private bool IfFileExists(FileDescriptor filePath) => _files.Any(n => n.Equals(filePath));
}
