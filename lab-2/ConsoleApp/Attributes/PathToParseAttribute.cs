namespace ConsoleApp.Attributes;

public class PathToParseAttribute : Attribute
{
    public FileInfo PathToParse { get; }

    public PathToParseAttribute(string pathToParse)
    {
        ArgumentException.ThrowIfNullOrEmpty(pathToParse, nameof(pathToParse));

        PathToParse = new FileInfo(pathToParse);
    }
}