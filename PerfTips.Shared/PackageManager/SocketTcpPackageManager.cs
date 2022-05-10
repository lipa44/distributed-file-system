using System.Net;
using System.Net.Sockets;
using PerfTips.Shared.Serializer;

namespace PerfTips.Shared.PackageManager;

public class SocketTcpPackageManager : IPackageManager
{
    private readonly ISerializer _serializer;
    private readonly int _bufferSize;

    public SocketTcpPackageManager(int bufferSize, ISerializer serializer)
    {
        _bufferSize = bufferSize;
        _serializer = serializer;
    }

    public ISerializer Serializer => _serializer;

    public Socket SendPackage<T>(T message, IPEndPoint endpoint)
    {
        var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        tcpSocket.Connect(endpoint);
        tcpSocket.Send(_serializer.Serialize(message));

        return tcpSocket;
    }

    public byte[] ReceivePackage(Socket socket)
    {
        var buffer = new byte[_bufferSize];
        var result = new List<byte>();

        do
        {
            var size = socket.Receive(buffer);

            result.AddRange(size < _bufferSize ? buffer.Take(size) : buffer);
        } while (socket.Available > 0);

        return result.ToArray();
    }
}