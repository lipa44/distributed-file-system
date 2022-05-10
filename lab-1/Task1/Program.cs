namespace Task1;

public class Program
{
    public static void Main()
    {
        var import = LibraryImport.Select();
        int sum = import.Sum(100, 128);
        Console.WriteLine(sum);
    }
}