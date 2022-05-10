namespace SourceGenerator.Helpers;

public interface IBracketsChecker
{
    bool IsBracketsBalanced(string[] code);
}