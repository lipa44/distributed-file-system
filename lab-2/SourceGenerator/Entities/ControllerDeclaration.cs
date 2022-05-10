using System.Collections.Generic;

namespace SourceGenerator.Entities;

public record ControllerDeclaration(
    string Name,
    string BaseUri,
    CodeScope Scope,
    IReadOnlyList<ParsedMethod> Methods)
{
    public string Name { get; } = Name;
    public string BaseUri { get; } = BaseUri;
    public CodeScope Scope { get; } = Scope;
    public IReadOnlyList<ParsedMethod> Methods { get; } = Methods;
}