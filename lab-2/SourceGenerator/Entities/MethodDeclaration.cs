namespace SourceGenerator.Entities;

public record MethodDeclaration(
    string AccessModifier,
    string Name,
    string ReturnType,
    HttpInfo HttpInfo,
    params MethodParam[] Params)
{
    public string AccessModifier { get; } = AccessModifier;
    public string Name { get; } = Name;
    public string ReturnType { get; } = ReturnType;
    public HttpInfo HttpInfo { get; } = HttpInfo;
    public MethodParam[] Params { get; } = Params;
}