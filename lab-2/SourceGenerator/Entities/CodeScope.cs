using SourceGenerator.Enums;

namespace SourceGenerator.Entities;

public record CodeScope(
    int Start,
    int End,
    CodeScopes ScopeType)
{
    public int Start { get; } = Start;
    public int End { get; } = End;
    public CodeScopes ScopeType { get; } = ScopeType;
}