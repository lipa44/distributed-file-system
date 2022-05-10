using System;

namespace SourceGenerator.Exceptions;

public class JavaParserException : Exception
{
    public static void ThrowIfNull(object? item, string itemName)
    {
        if (item is null) throw new ArgumentNullException(itemName);
    }

    public static void ThrowIfNullOrWhiteSpace(string item, string itemName)
    {
        if (string.IsNullOrWhiteSpace(item)) throw new ArgumentNullException(itemName);
    }
}