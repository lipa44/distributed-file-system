using System.Collections.Generic;

namespace SourceGenerator.Entities;

public record ParsedMethod(
    CodeScope Scope,
    MethodDeclaration Declaration,
    IReadOnlyList<string> Code)
{
    public CodeScope Scope { get; } = Scope;
    public MethodDeclaration Declaration { get; } = Declaration;
    public IReadOnlyList<string> Code { get; } = Code;
}