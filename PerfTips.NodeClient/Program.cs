using System.Net;
using System.Net.Sockets;
using AutoMapper;
using PerfTips.NodeClient.Commands;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared.Enums;
using PerfTips.Shared.PackageManager;
using PerfTips.Shared.Serializer;

const string server = "127.0.0.1";
const string relativePath = "/Users/lipa/Desktop/Nodes";

MapperConfiguration mapperConfig = new(cfg =>
{
    cfg.CreateMap<ServerCommands, INodeCommand>()
        .ConvertUsing((value, _) => value switch
        {
            ServerCommands.AddFile => new AddFileCommand(),
            ServerCommands.RemoveFile => new RemoveFileCommand(),
            ServerCommands.CleanNode => new CleanNodeCommand(),
            _ => new UnrecognizedCommand(),
        });
});

Console.Write("Enter port: ");
var port = int.Parse(Console.ReadLine()!);

/* Creating socket in waiting mode */
var tcpEndPoint = new IPEndPoint(IPAddress.Parse(server), port);
var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

socket.Bind(tcpEndPoint); // Binding our socket to concrete endpoint
Console.WriteLine($"Endpoint started listening at: {server}:{port}");
socket.Listen(); // Turning socket into waiting (listening) mode

ISerializer serializer = new Utf8Serializer();
IPackageManager packageManager = new SocketTcpPackageManager(serializer);
IMapper mapper = mapperConfig.CreateMapper();

TcpNodeInstance nodeInstance = new(relativePath, IPAddress.Parse(server), port, mapper, serializer);

try
{
    while (true)
    {
        /* Creating listener for our socket  */
        var listener = await socket.AcceptAsync();

        var package = packageManager.ReceivePackage(listener, tcpEndPoint);

        await nodeInstance.Execute(package, new CancellationTokenSource());
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Cancellation requested, node stopped");
}
catch (Exception e)
{
    Console.WriteLine(e);
}