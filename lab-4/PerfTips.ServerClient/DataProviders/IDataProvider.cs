using PerfTips.Shared.Enums;

namespace PerfTips.ServerClient.DataProviders;

public interface IDataProvider
{
    ServerCommands GetCommand();
    string AskData(string message);
}