namespace SourceGenerator.Entities;

public record HttpInfo(
    string HttpType,
    string Uri)
{
    public string HttpType { get; } = HttpType;
    public string Uri { get; } = Uri;
}