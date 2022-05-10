using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceGenerator.Helpers;

public class BracketsChecker : IBracketsChecker
{
    private const string OpeningBrackets = "([{";
    private const string ClosingBrackets = ")]}";

    private readonly Stack<int> _opened = new ();

    private bool _cantBeBalanced;

    public bool IsBalanced => !_cantBeBalanced && !_opened.Any();

    public bool IsBracketsBalanced(string[] code)
    {
        foreach (var str in code ?? throw new ArgumentNullException(nameof(code)))
        foreach (var ch in str ?? throw new ArgumentNullException(nameof(str)))
            PutChar(ch);

        return IsBalanced;
    }

    private void PutChar(char ch)
    {
        if (_cantBeBalanced) return;

        var index = OpeningBrackets.IndexOf(ch);

        if (index != -1)
        {
            _opened.Push(index);
            return;
        }

        index = ClosingBrackets.IndexOf(ch);

        if (index == -1) return;
        if (!_opened.Any() || _opened.Peek() != index)
        {
            _cantBeBalanced = true;
            _opened.Clear();
            _opened.TrimExcess();
            return;
        }

        _opened.Pop();
    }
}