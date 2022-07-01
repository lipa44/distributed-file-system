using PerfTips.Shared.Enums;

namespace PerfTips.ServerClient.DataProviders;

public class ConsoleDataProvider : IDataProvider
{
    public ServerCommands GetCommand()
    {
        Enum.TryParse<ServerCommands>(Console.ReadLine()!.Trim(), out var command);

        return command;
    }

    public string AskData(string message)
    {
        Console.Write(message);

        return Console.ReadLine()!.Trim();
    }
}