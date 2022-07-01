using System.Net;
using System.Net.Sockets;
using AutoMapper;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared.PackageManager;
using PerfTips.NodeClient;

var appSettings = Startup.AppSettings;
string server = appSettings.Server;

if (args.Any() && int.TryParse(args[0], out int port))
    port = int.Parse(args[0]);
else
{
    Console.Write("Port: ");
    port = int.Parse(Console.ReadLine()!);
}

var tcpEndPoint = new IPEndPoint(IPAddress.Parse(server), port);
var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

socket.Bind(tcpEndPoint);
Console.WriteLine($"Endpoint started listening at: {server}:{port}");
socket.Listen();

IPackageManager packageManager = Startup.PackageManager;
IMapper mapper = Startup.Mapper;

TcpNodeInstance nodeInstance = new(Path.Combine(Startup.DesktopPath, appSettings.RelativePath), IPAddress.Parse(server),
    port, mapper, packageManager);

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (s, e) =>
{
    cts.Cancel();
    e.Cancel = true;
};

try
{
    while (!cts.IsCancellationRequested)
    {
        using var listener = await socket.AcceptAsync(cts.Token);

        await nodeInstance.Execute(listener, cts);
    }

    cts.Token.ThrowIfCancellationRequested();
}
catch (OperationCanceledException)
{
    Console.WriteLine("Cancellation requested, node stopped");
}
catch (Exception e)
{
    Console.WriteLine(e);
}