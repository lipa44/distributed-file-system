using System.Net;
using AutoMapper;
using PerfTips.ServerClient.Commands;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.Enums;
using PerfTips.Shared.PackageManager;
using PerfTips.Shared.Serializer;

#region Summary

/* 
 * Анекдот про TCP/UTP
 *
 * TCP устанавливает чёткое соединение между клиентом и сервером (канал) и обеспечивает гарантию того, что информация будет доставлена
 * При ошибке или отсутствии ответа пакет будет отправляться ещё и ещё...
 * При этом TCP работает медленнее в связи с временем на установку соединения (плохо при широковещательном оповещении)
 *
 * UTP не гарантирует доставку пакетов, но он более лёгкий (хорош когда надо оповестить большое кол-во клиентов)
 */

#endregion

#region MayBeUsed

// IConfiguration config = new ConfigurationBuilder()
//     .AddJsonFile("appsettings.json")
//     .AddEnvironmentVariables()
//     .Build();
//
// var settings = config.GetRequiredSection(nameof(AppSettings)).Get<AppSettings>();
//
// Console.WriteLine($"Path = {settings.Path}");

#endregion

const int port = 8888;
const string server = "127.0.0.1";
const int bufferSize = 256;

MapperConfiguration mapperConfig = new(cfg =>
{
    cfg.CreateMap<ServerCommands, IServerCommand>()
        .ConvertUsing((value, _) => value switch
        {
            ServerCommands.AddFile => new AddFileCommand(),
            ServerCommands.AddNode => new AddNodeCommand(),
            ServerCommands.CleanNode => new CleanNodeCommand(),
            ServerCommands.BalanceNode => new BalanceNodeCommand(),
            ServerCommands.Exec => new ExecCommand(),
            ServerCommands.Quit => new QuitCommand(),
            ServerCommands.GetNodes => new GetNodesCommand(),
            ServerCommands.RemoveFile => new RemoveFileCommand(),
            _ => new UnrecognizedCommand(),
        });
});

IDataProvider commandsProvider = new ConsoleDataProvider();
ISerializer serializer = new Utf8Serializer();
IPackageManager packageManager = new SocketTcpPackageManager(bufferSize, serializer);
IMapper mapper = mapperConfig.CreateMapper();

try
{
    ServerInstance serverInstance = new (IPAddress.Parse(server), port, mapper, commandsProvider, packageManager);

    while (true)
    {
        var command = commandsProvider.GetCommand();
        await serverInstance.Execute(command, new CancellationTokenSource());
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Cancellation requested, program stopped");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
