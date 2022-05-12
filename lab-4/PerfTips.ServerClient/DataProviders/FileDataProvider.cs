using PerfTips.Shared.Enums;

namespace PerfTips.ServerClient.DataProviders;

public class FileDataProvider : IDataProvider
{
    private readonly Queue<Command> _commands = new();
    private Queue<string> _currentArguments = new();

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

        var command = _commands.Dequeue();
        _currentArguments = command.Arguments;

        Enum.TryParse<ServerCommands>(command.Name, out var serverCommand);

        return serverCommand;
    }

    public string AskData(string message) => _currentArguments.Dequeue();

    private void ParseFileToCommands()
    {
        foreach (var commandString in File.ReadAllLines(FilePath))
        {
            var commandWithArgs = new Queue<string>(commandString.Split(" "));

            _commands.Enqueue(new(commandWithArgs.Dequeue(), commandWithArgs));
        }
    }

    private class Command
    {
        public Command(string name, Queue<string> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public string Name { get; }
        public Queue<string> Arguments { get; }
    }
}