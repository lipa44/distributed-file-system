using System.Net;
using System.Net.Sockets;
using PerfTips.Shared.Enums;
using PerfTips.Shared.Serializer;

namespace PerfTips.Shared.PackageManager;

public class SocketTcpPackageManager : IPackageManager
{
    private readonly ISerializer _serializer;

    public SocketTcpPackageManager(ISerializer serializer)
    {
        _serializer = serializer;
    }

    public Socket SendPackage<T>(ServerCommands command, T message, IPEndPoint endpoint)
    {
        var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        tcpSocket.Connect(endpoint);
        tcpSocket.Send(_serializer.Serialize(message));

        return tcpSocket;
    }

    public byte[] ReceivePackage(Socket socket, IPEndPoint endpoint)
    {
        const int bufferSize = 256;

        var buffer = new byte[bufferSize];
        var result = new List<byte>();

        do
        {
            var size = socket.Receive(buffer);

            result.AddRange(size < bufferSize ? buffer.Take(size) : buffer);
        } while (socket.Available > 0);

        return result.ToArray();
    }
}