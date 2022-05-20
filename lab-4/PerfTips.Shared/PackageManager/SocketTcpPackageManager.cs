using System.Net;
using System.Net.Sockets;
using PerfTips.Shared.MessageRecords;
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

        var sizePackage = new byte[_bufferSize];
        var package = _serializer.Serialize(message);

        _serializer.Serialize(package.Length).CopyTo(sizePackage, 0);

        await tcpSocket.SendAsync(sizePackage);
        await tcpSocket.SendAsync(_serializer.Serialize(message));

        return tcpSocket;
    }

    public async Task<Socket> SendFile<T>(T message, Socket socket)
    {
        var sizePackage = new byte[_bufferSize];
        var package = _serializer.Serialize(message);

        _serializer.Serialize(package.Length).CopyTo(sizePackage, 0);

        await socket.SendAsync(sizePackage);
        await socket.SendAsync(_serializer.Serialize(message));

        return socket;
    }

    public async Task<FileMessage> ReceiveFile(Socket listener)
    {
        var packageSizeBuffer = new byte[_bufferSize];

        await listener.ReceiveAsync(packageSizeBuffer);

        var fileArray = new byte[_serializer.Deserialize<int>(packageSizeBuffer)];

        await listener.ReceiveAsync(fileArray);

        return _serializer.Deserialize<FileMessage>(fileArray);
    }

    public async Task<byte[]> ReceivePackage(Socket listener)
    {
        var packageSizeBuffer = new byte[_bufferSize];

        await listener.ReceiveAsync(packageSizeBuffer);
        var packageSize = _serializer.Deserialize<int>(packageSizeBuffer);

        var packageBuffer = new byte[packageSize];

        await listener.ReceiveAsync(packageBuffer);

        return packageBuffer;
    }
}