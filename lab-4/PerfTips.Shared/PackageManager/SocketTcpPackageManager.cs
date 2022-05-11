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

    public async Task<byte[]> ReceivePackage(IPEndPoint ipEndPoint)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.Bind(ipEndPoint);
        socket.Listen();

        var listener = await socket.AcceptAsync();
        var buffer = new byte[_bufferSize];
        var result = new List<byte>();

        do
        {
            var size = listener.Receive(buffer);
            result.AddRange(size < _bufferSize ? buffer.Take(size) : buffer);
        } while (listener.Available > 0);

        listener.Shutdown(SocketShutdown.Both);
        listener.Close();

        return result.ToArray();
    }

    public byte[] ReceivePackage(Socket listener)
    {
        var buffer = new byte[_bufferSize];
        var result = new List<byte>();

        do
        {
            var size = listener.Receive(buffer);
            result.AddRange(size < _bufferSize ? buffer.Take(size) : buffer);
        } while (listener.Available > 0);

        return result.ToArray();
    }
}