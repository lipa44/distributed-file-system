namespace SourceGenerator.Entities;

public record EntityDeclaration(
    string Name,
    params Param[] Params)
{
    public string Name { get; } = Name;
    public Param[] Params { get; } = Params;
}