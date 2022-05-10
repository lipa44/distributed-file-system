using PerfTips.Shared.Enums;

namespace PerfTips.ServerClient.DataProviders;

public interface IDataProvider
{
    ServerCommands GetCommand();
    public string AskForData(string message);
}