using System.Buffers;
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

    public async Task<Socket> SendPackage<T>(T message, IPEndPoint endpoint)
    {
        var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        await tcpSocket.ConnectAsync(endpoint);
        await tcpSocket.SendAsync(_serializer.Serialize(message));

        return tcpSocket;
    }

    public async Task<byte[]> ReceivePackage(Socket listener)
    {
        var result = new List<byte>();
        var buffer = ArrayPool<byte>.Shared.Rent(_bufferSize);

        do
        {
            var size = await listener.ReceiveAsync(buffer);
            result.AddRange(size < _bufferSize ? buffer.Take(size) : buffer);
        } while (listener.Available > 0);

        ArrayPool<byte>.Shared.Return(buffer);

        return result.ToArray();
    }
}