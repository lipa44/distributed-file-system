﻿using System.Text;

namespace PerfTips.FilesGenerator;

public static class Program
{
    private static readonly string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string CommandsPath = Path.Combine(Desktop, "Commands.txt");
    private static readonly string FilesPath = Path.Combine(Desktop, "NodeFiles");
    private const int FilesAmount = 3000;
    private const int BalanceAmount = 1;

    private static readonly IReadOnlyList<string> NodeNames = new List<string> { "A", "B", "C" };

    public static void Main()
    {
        if (!Directory.Exists(Path.Combine(Desktop, "Nodes")))
            Directory.CreateDirectory(Path.Combine(Desktop, "Nodes"));

        if (!Directory.Exists(FilesPath))
            Directory.CreateDirectory(FilesPath);

        var stringBuilder = new StringBuilder();
        for (var i = 0; i < FilesAmount; i++)
            File.WriteAllText($"{Path.Combine(FilesPath, $"file{i}.txt")}", stringBuilder.Append("aaa").ToString());

        stringBuilder.Clear();

        File.Delete(CommandsPath);

        File.AppendAllText(CommandsPath, "AddNode A 8887 20000000\n");
        File.AppendAllText(CommandsPath, "AddNode B 8886 25000000\n");
        File.AppendAllText(CommandsPath, "AddNode C 8885 24000000\n");

        for (var i = 0; i < FilesAmount; i++)
            File.AppendAllText(CommandsPath, $@"AddFile {Path.Combine(FilesPath, $"file{i}.txt")} {GetRandomNode()} file{i}" + "\n");

        stringBuilder.Append("BalanceNode\n");
        for (var i = 0; i < BalanceAmount; i++)
            File.AppendAllText(CommandsPath, stringBuilder.ToString());
    }

    private static string GetRandomNode() => NodeNames[new Random().Next(NodeNames.Count)];
}