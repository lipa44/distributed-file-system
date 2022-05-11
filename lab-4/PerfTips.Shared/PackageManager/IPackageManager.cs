using System.Net;
using System.Net.Sockets;
using PerfTips.Shared.Serializer;

namespace PerfTips.Shared.PackageManager;

public interface IPackageManager
{
    ISerializer Serializer { get; }
    Socket SendPackage<T>(T message, IPEndPoint endpoint);
    Task<byte[]> ReceivePackage(IPEndPoint endPoint);
    byte[] ReceivePackage(Socket listener);
}