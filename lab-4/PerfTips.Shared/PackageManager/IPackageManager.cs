using System.Net;
using System.Net.Sockets;
using PerfTips.Shared.MessageRecords;
using PerfTips.Shared.Serializer;

namespace PerfTips.Shared.PackageManager;

public interface IPackageManager
{
    ISerializer Serializer { get; }
    Task<Socket> SendPackage<T>(T message, IPEndPoint endpoint);
    Task<Socket> SendFile<T>(T message, Socket socket);
    Task<byte[]> ReceivePackage(Socket listener);
    Task<FileMessage> ReceiveFile(Socket listener);
}