using AutoMapper;
using Microsoft.Extensions.Configuration;
using PerfTips.NodeClient.Commands;
using PerfTips.Shared.Enums;
using PerfTips.Shared.PackageManager;
using PerfTips.Shared.Serializer;
using AppSettings = PerfTips.NodeClient.Helpers.AppSettings;

namespace PerfTips.NodeClient;

public static class Startup
{
    private static readonly IConfiguration Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    public static readonly IMapper Mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<ServerCommands, INodeCommand>()
            .ConvertUsing((value, _) => value switch
            {
                ServerCommands.AddFile => new AddFileCommand(),
                ServerCommands.RemoveFile => new RemoveFileCommand(),
                ServerCommands.CleanNode => new CleanNodeCommand(),
                _ => new UnrecognizedCommand(),
            });
    }).CreateMapper();

    
    public static readonly ISerializer Serializer = new Utf8Serializer();

    public static readonly IPackageManager PackageManager = new SocketTcpPackageManager(AppSettings.BufferSize, Serializer);

    public static AppSettings AppSettings => Config.GetRequiredSection(nameof(AppSettings)).Get<AppSettings>();
}