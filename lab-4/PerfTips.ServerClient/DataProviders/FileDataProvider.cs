using PerfTips.Shared.Enums;

namespace PerfTips.ServerClient.DataProviders;

public class FileDataProvider : IDataProvider
{
    private readonly List<Command> _commands = new();
    private string[] _currentArguments;

    public FileDataProvider(string filePath)
    {
        FilePath = filePath;

        ParseFileToCommands();
    }

    public string FilePath { get; }

    public ServerCommands GetCommand()
    {
        if (!_commands.Any())
            return ServerCommands.Quit;

        var command = _commands.FirstOrDefault();
        _currentArguments = command?.Arguments ?? Array.Empty<string>();
        _commands.RemoveAt(0);

        Enum.TryParse<ServerCommands>(command?.Name ?? string.Empty, out var serverCommand);

        return serverCommand;
    }

    public string AskForData(string message)
    {
        var argument = _currentArguments.FirstOrDefault();
        _currentArguments = _currentArguments.Skip(1).ToArray();

        return argument;
    }

    private void ParseFileToCommands()
    {
        foreach (var commandString in File.ReadAllLines(FilePath))
        {
            var commandWithArgs = commandString.Split(" ");

            _commands.Add(new Command(
                name: commandWithArgs.FirstOrDefault(),
                arguments: commandWithArgs.Skip(1).ToArray()));
        }
    }

    private class Command
    {
        public Command(string name, params string[] arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public string? Name { get; }
        public string[] Arguments { get; }
    }
}