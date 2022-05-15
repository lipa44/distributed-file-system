using System.Net;
using System.Net.Sockets;
using AutoMapper;
using PerfTips.NodeClient.TcpNode;
using PerfTips.Shared.PackageManager;

namespace PerfTips.NodeClient;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var appSettings = Startup.AppSettings;
        string server = appSettings.Server;

        // Console.Write("Enter port: ");
        var port = int.Parse(args[0]);

        var tcpEndPoint = new IPEndPoint(IPAddress.Parse(server), port);
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.Bind(tcpEndPoint);
        Console.WriteLine($"Endpoint started listening at: {server}:{port}");
        socket.Listen();

        IPackageManager packageManager = Startup.PackageManager;
        IMapper mapper = Startup.Mapper;

        TcpNode.TcpNode node = new (appSettings.RelativePath, IPAddress.Parse(server), port, mapper, packageManager);

        var cts = new CancellationTokenSource();
        
        Console.CancelKeyPress += (s, e) =>
        {
            Console.WriteLine("Canceling...");
            cts.Cancel();
            e.Cancel = true;
        };

        try
        {
            while (!cts.IsCancellationRequested)
            {
                /* Creating listener for our socket  */
                var listener = await socket.AcceptAsync(cts.Token);

                var package = packageManager.ReceivePackage(listener);

                await node.Execute(listener, package, new CancellationTokenSource());
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
    }
}