using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SourceGenerator.Entities;
using SourceGenerator.Enums;
using SourceGenerator.Exceptions;
using SourceGenerator.Helpers;

namespace SourceGenerator.Parsers;

public class JavaParser : IParser
{
    public const string SearchExtension = "java";

    private const string JavaMethodRegexString = @"(?<AccessModifier>(public|private|protected))[^\w]*(?<KeyWords>(static|final|native|synchronized|abstract|transient)*)*[^\w]*(?<ReturnType>(\S+))[^\w]*(?<NameAndParams>(.*[^{]))";
    private const string JavaMethodParamsRegexString = @"([\s]*(?<httpType>(\@[\w]*))?[\s]?(?<paramType>([\w]*)){1}[\s]+(?<paramName>([\w]*)){1})";
    private const string JavaPropertyRegexString = @"((?<accessModifier>(public|private|protected))[\s]+(?<propType>([\w]*))[\s]+(?<propName>([\w]*))\;)";
    private const string JavaHttpAnnotationString = @"[(*)]";

    private static readonly Regex JavaMethodRegex = new (JavaMethodRegexString);
    private static readonly Regex JavaMethodParamsRegex = new (JavaMethodParamsRegexString);
    private static readonly Regex JavaPropertyRegex = new (JavaPropertyRegexString);
    private static readonly Regex JavaHttpAnnotationRegex = new (JavaHttpAnnotationString);

    public IReadOnlyCollection<FileInfo> ControllerFiles { get; private set; }
    public IReadOnlyCollection<FileInfo> EntityFiles { get; private set; }

    private static IBracketsChecker _bracketsChecker;

    public JavaParser(IBracketsChecker checker)
    {
        _bracketsChecker = checker ?? throw new ArgumentNullException(nameof(checker));
    }

    public IReadOnlyList<EntityDeclaration> ParseEntityDeclarations(List<FileInfo> fileInfos)
    {
        JavaParserException.ThrowIfNull(fileInfos, nameof(fileInfos));

        var entities = new List<EntityDeclaration>();

        foreach (var javaFileCode in fileInfos.Select(fileInfo => File.ReadAllLines(fileInfo.FullName)))
        {
            if (!_bracketsChecker.IsBracketsBalanced(javaFileCode))
                throw new Exception($"Code's brackets are not balanced. Try to use better language than {SearchExtension} :)");

            var entityParams = new List<Param>();
            var entityScopes = GetScopes(javaFileCode);

            var entityClassScope = entityScopes.SingleOrDefault(s => s.ScopeType is CodeScopes.ClassScope)
                                       ?? throw new Exception("Your entity class doesn't have any class declaration");

            foreach (var codeLine in javaFileCode)
            {
                if (!JavaPropertyRegex.IsMatch(codeLine)) continue;

                var matchCollection = JavaPropertyRegex.Matches(codeLine);
                var propType = TryFindKnownCSharpType(matchCollection[0].Groups["propType"].Value);
                var propName = matchCollection[0].Groups["propName"].Value;

                entityParams.Add(new Param(propName, propType));
            }

            entities.Add(ParseEntityClassSignature(_getScopeLines(javaFileCode, entityClassScope), entityParams));
        }

        return entities;
    }

    private EntityDeclaration ParseEntityClassSignature(string[] entityClassLines, List<Param> @params)
    {
        JavaParserException.ThrowIfNull(entityClassLines, nameof(entityClassLines));
        JavaParserException.ThrowIfNull(@params, nameof(@params));

        var name = new Regex(@"(?<accessModifier>([\w]*)){1}[\s]+(?<propType>(class)){1}[\s]+(?<className>([\w]*)){1}")
            .Matches(entityClassLines[1])[0].Groups["className"].Value;

        return new EntityDeclaration(name, @params.ToArray());
    }

    public IReadOnlyList<ControllerDeclaration> ParseControllerDeclarations(List<FileInfo> fileInfos)
    {
        JavaParserException.ThrowIfNull(fileInfos, nameof(fileInfos));

        var controllers = new List<ControllerDeclaration>();

        foreach (var javaFileCode in fileInfos.Select(fileInfo => File.ReadAllLines(fileInfo.FullName)))
        {
            if (!_bracketsChecker.IsBracketsBalanced(javaFileCode))
                throw new Exception($"Code's brackets are not balanced. Try to use better language than {SearchExtension} :)");

            var parsedMethods = new List<ParsedMethod>();
            var controllerScopes = GetScopes(javaFileCode);

            var controllerClassScope = controllerScopes.SingleOrDefault(s => s.ScopeType is CodeScopes.ClassScope)
                                       ?? throw new Exception("Your controller class doesn't have any class declaration");

            foreach (var methodScope in controllerScopes)
            {
                if (methodScope.ScopeType is not CodeScopes.MethodScope) continue;

                var methodLines = _getScopeLines(javaFileCode, methodScope);
                var methodDeclaration = ParseMethodSignature(methodLines);

                parsedMethods.Add(new ParsedMethod(methodScope, methodDeclaration, methodLines));
            }

            var controllerClassLines = _getScopeLines(javaFileCode, controllerClassScope);

            controllers.Add(
                ParseControllerClassSignature(controllerClassLines, controllerClassScope, parsedMethods));
        }

        return controllers;
    }

    public List<CodeScope> GetScopes(string[] code)
    {
        JavaParserException.ThrowIfNull(code, nameof(code));

        Stack<int> openingBrackets = new();
        List<CodeScope> methodScopes = new();
        for (int i = 0, nesting = 1; i < code.Length; ++i)
        {
            bool containsOpening = code[i].Contains('{'),
                containsClosing = code[i].Contains('}');

            if (!containsOpening && !containsClosing) continue;

            if (containsOpening && containsClosing)
            {
                if (code[i].IndexOf('{') < code[i].IndexOf('}'))
                {
                    methodScopes.Add(new CodeScope(i, i, CodeScopes.InlineScope));
                }
                else
                {
                    methodScopes.Add(new CodeScope(openingBrackets.Pop(), i, GetScopeByNesting(nesting)));
                    openingBrackets.Push(i);
                }
            }
            else if (containsOpening)
            {
                ++nesting;
                openingBrackets.Push(i);
            }
            else if (containsClosing)
            {
                methodScopes.Add(new CodeScope(openingBrackets.Pop(), i, GetScopeByNesting(--nesting)));
            }
        }

        return methodScopes;
    }

    private HttpInfo ParseHttpAttribute(string[] javaMethod)
    {
        if (!javaMethod.Any())
            throw new Exception("Java method to parse is empty");

        var javaHttpAnnotation = javaMethod[0];

        if (!JavaHttpAnnotationRegex.IsMatch(javaHttpAnnotation))
            return new HttpInfo(_cutHttpRequestType(javaHttpAnnotation), Uri: string.Empty);

        int openBracketIndex = javaHttpAnnotation.IndexOf('('),
            closeBracketIndex = javaHttpAnnotation.LastIndexOf(')');

        var uri = javaHttpAnnotation
            .Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1)
            .Replace("\"", string.Empty);

        return new HttpInfo(_cutHttpRequestType(javaHttpAnnotation.Remove(openBracketIndex)), Uri: uri);
    }

    private MethodDeclaration ParseMethodSignature(string[] javaMethod)
    {
        JavaParserException.ThrowIfNull(javaMethod, nameof(javaMethod));

        var methodSignature = javaMethod[1];
        var matchCollection = JavaMethodRegex.Matches(methodSignature);

        var accessModifier = matchCollection[0].Groups["AccessModifier"].Value;
        var nameAndParams = matchCollection[0].Groups["NameAndParams"].Value.Trim();
        var returnType = TryFindKnownCSharpType(matchCollection[0].Groups["ReturnType"].Value);
        var name = nameAndParams.Remove(nameAndParams.IndexOf('('), nameAndParams.Length - nameAndParams.IndexOf('('));

        var param = new List<MethodParam>();
        foreach (Match match in JavaMethodParamsRegex.Matches(nameAndParams.Replace(name, string.Empty)))
        {
            param.Add(new MethodParam(
                new Param(match.Groups["paramName"].Value,
                    TryFindKnownCSharpType(match.Groups["paramType"].Value)),
                match.Groups["httpType"].Value.Replace("@", string.Empty)));
        }

        var httpRequestInfo = ParseHttpAttribute(javaMethod);

        return new MethodDeclaration(accessModifier, name, returnType, httpRequestInfo, param.ToArray());
    }

    private ControllerDeclaration ParseControllerClassSignature(string[] javaMethod, CodeScope scope, List<ParsedMethod> methods)
    {
        JavaParserException.ThrowIfNull(javaMethod, nameof(javaMethod));
        JavaParserException.ThrowIfNull(methods, nameof(methods));

        var httpInfo = ParseHttpAttribute(javaMethod);

        var controllerSignature = javaMethod[1];
        var matchCollection = new Regex(@"class[^\w]*(?<ControllerName>([\w]*))").Matches(controllerSignature);

        var className = matchCollection[0].Groups["ControllerName"].Value;

        return new ControllerDeclaration(className, httpInfo.Uri, scope, methods);
    }

    public IParser CalculateFilesToParse(FileInfo directoryPath)
    {
        JavaParserException.ThrowIfNull(directoryPath, nameof(directoryPath));

        if (!Directory.Exists(directoryPath.Directory?.FullName))
            throw new ArgumentException("Directory doesn't exist");

        ControllerFiles = Directory
            .GetFiles(directoryPath.FullName, "*Controller." + SearchExtension, SearchOption.AllDirectories)
            .Select(filePath => new FileInfo(filePath)).ToList();

        EntityFiles = Directory
            .GetFiles(directoryPath.FullName, "*Entity." + SearchExtension, SearchOption.AllDirectories)
            .Select(filePath => new FileInfo(filePath)).ToList();

        return this;
    }

    private CodeScopes GetScopeByNesting(int nesting) =>
        nesting switch
        {
            1 => (CodeScopes) Enum.Parse(typeof(CodeScopes), $"{nesting}"),
            2 => (CodeScopes) Enum.Parse(typeof(CodeScopes), $"{nesting}"),
            _ => CodeScopes.InnerScope
        };

    private readonly Func<string[], CodeScope, string[]> _getScopeLines =
        (javaFileCode, scope) => javaFileCode
            .Skip(scope.Start - 1)
            .Take(scope.End - scope.Start + 2)
            .ToArray();

    private readonly Func<string, string> _cutHttpRequestType =
        httpAnnotationString => httpAnnotationString
            .Trim()
            .Replace("@", string.Empty)
            .Replace("Mapping", string.Empty);

    private string TryFindKnownCSharpType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentNullException(nameof(type));

        return type switch
        {
            "boolean" => "bool",
            "Integer" => "int",
            _ => IsKnownCSharpType(type) ? type.ToLower() : type
        };
    }

    private bool IsKnownCSharpType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentNullException(nameof(type));

        switch (type.ToLower())
        {
            case "int":
            case "integer":
            case "uint":
            case "ushort":
            case "long":
            case "ulong":
            case "double":
            case "float":
            case "decimal":
            case "string":
            case "char":
            case "bool":
            case "byte":
            case "short":
            case "object":
            case "void":
                return true;
            
            default:
                return false;
        }
    }
}