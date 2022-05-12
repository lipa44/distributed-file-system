using PerfTips.Shared.Enums;
using Spectre.Console;

namespace PerfTips.ServerClient.DataProviders;

public class SpectreConsoleDataProvider : IDataProvider
{
    private readonly IEnumerable<ServerCommands> _commands =
        Enum.GetValues(typeof(ServerCommands)).Cast<ServerCommands>().Skip(1);

    public ServerCommands GetCommand() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<ServerCommands>()
                .Title("Choose one of the following [green]command[/].")
                .PageSize(_commands.Count())
                .UseConverter(command => $"* {command}")
                .AddChoices(_commands));

    public string AskData(string message) => AnsiConsole.Ask<string>(message);
}