using System.Net;
using AutoMapper;
using PerfTips.ServerClient;
using PerfTips.ServerClient.DataProviders;
using PerfTips.ServerClient.TcpServer;
using PerfTips.Shared.PackageManager;

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

var appSettings = Startup.AppSettings;

IDataProvider commandsProvider = Startup.DataProvider;
IPackageManager packageManager = Startup.PackageManager;
IMapper mapper = Startup.Mapper;

// var cts = new CancellationTokenSource();
//
// Console.CancelKeyPress += (s, e) =>
// {
//     Console.WriteLine("Canceling...");
//     cts.Cancel();
//     e.Cancel = true;
// };

try
{
    ServerInstance serverInstance = new (IPAddress.Parse(appSettings.Server), appSettings.Port, mapper, commandsProvider, packageManager);

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
