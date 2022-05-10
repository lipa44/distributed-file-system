using PerfTips.ServerClient.DataProviders;
using PerfTips.Shared.PackageManager;

namespace PerfTips.ServerClient.Commands;

public class AddNodeCommand : IServerCommand
{
    public Task Execute(ServerInstance serverInstance, IPackageManager packageManager, IDataProvider dataProvider,
        CancellationTokenSource token)
    {
        Console.WriteLine("***Enter node's info***");

        var name = dataProvider.AskForData("Name: ");

        var port = int.Parse(dataProvider.AskForData("Port: "));
        var size = int.Parse(dataProvider.AskForData("MaxSize: "));

        var tcpNode = new NodeInfo(name, port, size);

        serverInstance.AddNode(tcpNode);

        Console.WriteLine($"\nNode {tcpNode} successfully added!\n");

        return Task.CompletedTask;
    }
}