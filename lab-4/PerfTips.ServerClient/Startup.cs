using AutoMapper;
using Microsoft.Extensions.Configuration;
using PerfTips.ServerClient.Commands;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.Helpers;
using PerfTips.Shared.Enums;
using PerfTips.Shared.PackageManager;
using PerfTips.Shared.Serializer;

namespace PerfTips.ServerClient;

public static class Startup
{
    private static readonly IConfiguration Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    public static readonly IMapper Mapper = new MapperConfiguration
    (cfg =>
    {
        cfg.CreateMap<ServerCommands, IServerCommand>()
            .ConvertUsing((value, _) => value switch
            {
                ServerCommands.AddFile => new AddFileCommand(),
                ServerCommands.AddNode => new AddNodeCommand(),
                ServerCommands.CleanNode => new CleanNodeCommand(),
                ServerCommands.BalanceNode => new BalanceNodeCommand(),
                ServerCommands.Quit => new QuitCommand(),
                ServerCommands.GetNodes => new GetNodesCommand(),
                ServerCommands.RemoveFile => new RemoveFileCommand(),
                _ => new UnrecognizedCommand(),
            });
    }).CreateMapper();

    public static readonly IDataProvider DataProvider = new FileDataProvider(@"C:\Users\user.local\Desktop\Commands.txt");

    public static readonly ISerializer Serializer = new Utf8Serializer();

    public static readonly IPackageManager PackageManager = new SocketTcpPackageManager(AppSettings.BufferSize, Serializer);

    public static AppSettings AppSettings => Config.GetRequiredSection(nameof(AppSettings)).Get<AppSettings>();
}