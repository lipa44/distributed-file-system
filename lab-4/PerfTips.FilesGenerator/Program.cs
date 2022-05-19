using System.Text;

namespace PerfTips.FilesGenerator;

public static class Program
{
    private static readonly string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string CommandsPath = Path.Combine(Desktop, "Commands.txt");
    private static readonly string FilesPath = Path.Combine(Desktop, "NodeFiles");
    private const int FilesAmount = 1000;

    private static readonly List<string> NodeNames = new() { "A", "B", "C" };

    public static void Main()
    {
        if (!Directory.Exists(Path.Combine(Desktop, "Nodes")))
            Directory.CreateDirectory(Path.Combine(Desktop, "Nodes"));

        if (!Directory.Exists(FilesPath))
            Directory.CreateDirectory(FilesPath);

        for (var i = 0; i < FilesAmount; i++)
            File.WriteAllLines($"{Path.Combine(FilesPath, "file{i}.txt")}", new []{ string.Concat(Enumerable.Repeat("aaa", i + 1)) });

        File.Delete(CommandsPath);
        using var file = File.Open(CommandsPath, FileMode.CreateNew, FileAccess.ReadWrite);

        file.Write(Encoding.UTF8.GetBytes("AddNode A 8887 20000000\n"));
        file.Write(Encoding.UTF8.GetBytes("AddNode B 8886 25000000\n"));
        file.Write(Encoding.UTF8.GetBytes("AddNode C 8885 24000000\n"));

        for (var i = 0; i < FilesAmount; i++)
            file.Write(Encoding.UTF8.GetBytes($@"AddFile {Path.Combine(FilesPath, "file{i}.txt")} {GetRandomNode()} file{i}" + "\n"));

        file.Write(Encoding.UTF8.GetBytes("BalanceNode\n"));
    }

    private static string GetRandomNode() => NodeNames[new Random().Next(NodeNames.Count)];
}