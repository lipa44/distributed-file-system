using System.Net;
using System.Net.Sockets;
using PerfTips.Shared.Enums;

namespace PerfTips.Shared.PackageManager;

public interface IPackageManager
{
    Socket SendPackage<T>(ServerCommands command, T message, IPEndPoint endpoint);
    byte[] ReceivePackage(Socket socket, IPEndPoint endpoint);
}