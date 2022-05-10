namespace SourceGenerator.Entities;

public record Param(
    string Name,
    string Type)
{
    public string Name { get; } = Name;
    public string Type { get; } = Type;
}