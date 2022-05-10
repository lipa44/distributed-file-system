namespace SourceGenerator.Entities;

public record MethodParam(
    Param Param,
    string HttpType)
{
    public Param Param { get; } = Param;
    public string HttpType { get; } = HttpType;
}