using System.Net;
using System.Net.Sockets;
using AutoMapper;
using PerfTips.NodeClient;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared.PackageManager;
using PerfTips.Shared.Serializer;

var appSettings = Startup.AppSettings;
string server = appSettings.Server;

Console.Write("Enter port: ");
var port = int.Parse(Console.ReadLine()!);


var tcpEndPoint = new IPEndPoint(IPAddress.Parse(server), port);
var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

socket.Bind(tcpEndPoint);
Console.WriteLine($"Endpoint started listening at: {server}:{port}");
socket.Listen();

ISerializer serializer = Startup.Serializer;
IPackageManager packageManager = Startup.PackageManager;
IMapper mapper = Startup.Mapper;

TcpNodeInstance nodeInstance = new (appSettings.RelativePath, IPAddress.Parse(server), port, mapper, serializer);

try
{
    while (true)
    {
        /* Creating listener for our socket  */
        var listener = await socket.AcceptAsync();

        var package = packageManager.ReceivePackage(listener);

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