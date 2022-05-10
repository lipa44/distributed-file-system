using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Entities;
using SourceGenerator.Exceptions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceGenerator.Rewriters;

public class MethodsCreator : CSharpSyntaxRewriter
{
    private readonly string _uri;
    private readonly string _port;

    private ControllerDeclaration ControllerDeclaration { get; set; }

    public MethodsCreator(string port, string uri = "http://localhost")
    {
        JavaParserException.ThrowIfNullOrWhiteSpace(port, nameof(port));
        JavaParserException.ThrowIfNullOrWhiteSpace(uri, nameof(uri));

        _port = port;
        _uri = uri;
    }

    public MethodsCreator WithController(ControllerDeclaration controllerDeclaration)
    {
        JavaParserException.ThrowIfNull(controllerDeclaration, nameof(controllerDeclaration));

        ControllerDeclaration = controllerDeclaration;

        return this;
    }

    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var methods = new List<MethodDeclarationSyntax>();

        foreach (var parsedMethod in ControllerDeclaration.Methods)
        {
            // 1. SyntaxTokenList - modifiers (access ones)
            var modifiers = TokenList(
                Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword),
                Token(SyntaxKind.AsyncKeyword));

            // 2. TypeSyntax - returnType
            var returnType = GenericName(
                    Identifier(TriviaList(), "Task", TriviaList()))
                .WithTypeArgumentList(TypeArgumentList()
                    .WithArguments(SeparatedList(new[]
                    {
                        ParseTypeName(parsedMethod.Declaration.ReturnType)
                    })));

            // 3. SyntaxToken - identifier
            var identifier = ParseToken(parsedMethod.Declaration.Name);

            // 4. ParameterListSyntax - parameterList
            var parameterList = ParameterList(SeparatedList(
                parsedMethod.Declaration.Params.Select(methodParam =>
                    Parameter(ParseToken(methodParam.Param.Name))
                        .WithType(ParseTypeName(methodParam.Param.Type)))));

            // MethodDeclaration creating
            var method = MethodDeclaration(returnType, identifier)
                .WithModifiers(modifiers)
                .WithParameterList(parameterList)
                .WithBody(
                    Block(
                        LocalDeclarationStatement(
                            VariableDeclaration(
                                    IdentifierName($"{parsedMethod.Declaration.ReturnType}"))
                                .WithVariables(
                                    SingletonSeparatedList(
                                        VariableDeclarator(
                                                Identifier("result"))
                                            .WithInitializer(
                                                EqualsValueClause(
                                                    LiteralExpression(SyntaxKind.DefaultLiteralExpression)))))),
                        ParseParametersForJsonSerialization(parsedMethod.Declaration.Params),
                        TryStatement(
                                SingletonList(
                                    CatchClause()
                                        .WithDeclaration(
                                            CatchDeclaration(
                                                    IdentifierName("Exception"))
                                                .WithIdentifier(
                                                    Identifier("e")))
                                        .WithBlock(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    ExpressionStatement(
                                                        ConsoleWriteLineExpression()
                                                            .WithArgumentList(
                                                                ArgumentList(
                                                                    SingletonSeparatedList(
                                                                        Argument(
                                                                            MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                IdentifierName("e"),
                                                                                IdentifierName("Message"))))))))))))
                            .WithBlock(
                                Block(
                                    RequestUriExpression(ControllerDeclaration, parsedMethod),
                                    RequestBodyExpression(ControllerDeclaration, parsedMethod),
                                    HttpClientRequestExpression(ControllerDeclaration, parsedMethod))),
                        ReturnStatement(
                            IdentifierName("result"))))
                .NormalizeWhitespace();

            methods.Add(method);
        }

        return node.AddMembers(methods.ToArray()).NormalizeWhitespace();
    }

    private SyntaxNodeOrToken[] AddMethodParametersToRequest(ParsedMethod method,
        ControllerDeclaration controllerDeclaration)
    {
        JavaParserException.ThrowIfNull(method, nameof(method));
        JavaParserException.ThrowIfNull(controllerDeclaration, nameof(controllerDeclaration));

        var tokens = new List<SyntaxNodeOrToken>
        {
            Argument(
                InterpolatedStringExpression(
                        Token(SyntaxKind.InterpolatedStringStartToken))
                    .WithContents(
                        SingletonList<InterpolatedStringContentSyntax>(
                            InterpolatedStringText()
                                .WithTextToken(
                                    Token(
                                        TriviaList(),
                                        SyntaxKind.InterpolatedStringTextToken,
                                        $"{_uri}:{_port}/{controllerDeclaration.BaseUri}{BuildRequestStringWithParams(method)}",
                                        $"{_uri}:{_port}/{controllerDeclaration.BaseUri}{BuildRequestStringWithParams(method)}",
                                        TriviaList())))))
        };

        if (method.Declaration.Params.Length == 0 || method.Declaration.Params.All(p => p.HttpType != "RequestBody"))
            return tokens.ToArray();

        tokens.Add(Token(SyntaxKind.CommaToken));
        tokens.Add(Argument(IdentifierName("contentBody")));

        return tokens.ToArray();
    }

    private LocalDeclarationStatementSyntax ParseParametersForJsonSerialization(MethodParam[] methodParams)
    {
        JavaParserException.ThrowIfNull(methodParams, nameof(methodParams));

        if (methodParams.Length == 0 || methodParams.All(p => p.HttpType != "RequestBody"))
            return LocalDeclarationStatement(
                VariableDeclaration(
                        IdentifierName("object"))
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(
                                Identifier("contentBody = default")))));

        var builder = new StringBuilder();
        if (methodParams.Length != 1)
        {
            builder.Append(@"contentBody = new {");

            foreach (var (param, httpType) in methodParams)
            {
                if (httpType != "RequestBody") continue;

                builder.Append($" {param.Name},");
            }

            // removing last comma
            builder.Remove(builder.Length - 1, 1);
            builder.Append(@" }");
        }
        else
        {
            builder.Append(@"contentBody = ");

            var (param, httpType) = methodParams.Single();

            if (httpType == "RequestBody")
                builder.Append($" {param.Name}");
        }

        return LocalDeclarationStatement(
            VariableDeclaration(
                    IdentifierName("object"))
                .WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator(
                            Identifier($"{builder}")))));
    }

    private ExpressionStatementSyntax RequestUriExpression(ControllerDeclaration controllerDeclaration,
        ParsedMethod parsedMethod)
    {
        JavaParserException.ThrowIfNull(controllerDeclaration, nameof(controllerDeclaration));
        JavaParserException.ThrowIfNull(parsedMethod, nameof(parsedMethod));

        return ExpressionStatement(
            ConsoleWriteLineWithContentsExpression(List(
                GetUriParams(controllerDeclaration, parsedMethod))));
    }

    private ExpressionStatementSyntax RequestBodyExpression(ControllerDeclaration controllerDeclaration,
        ParsedMethod parsedMethod)
    {
        JavaParserException.ThrowIfNull(controllerDeclaration, nameof(controllerDeclaration));
        JavaParserException.ThrowIfNull(parsedMethod, nameof(parsedMethod));

        return ExpressionStatement(
            ConsoleWriteLineWithContentsExpression(List(
                new InterpolatedStringContentSyntax[]
                {
                    InterpolatedStringText()
                        .WithTextToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.InterpolatedStringTextToken,
                                "Request Body: ",
                                "Request Body: ",
                                TriviaList())),
                    Interpolation(
                        IdentifierName("contentBody"))
                })));
    }

    private ExpressionStatementSyntax HttpClientRequestExpression(
        ControllerDeclaration controllerDeclaration,
        ParsedMethod parsedMethod)
    {
        JavaParserException.ThrowIfNull(controllerDeclaration, nameof(controllerDeclaration));
        JavaParserException.ThrowIfNull(parsedMethod, nameof(parsedMethod));

        return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName("result"),
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            InvocationExpression(
                                    IdentifierName(
                                        Identifier(
                                            TriviaList(),
                                            SyntaxKind.AwaitKeyword,
                                            "await",
                                            "await",
                                            TriviaList())))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                AwaitExpression(
                                                    InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                ObjectCreationExpression(
                                                                        IdentifierName("HttpClient"))
                                                                    .WithArgumentList(
                                                                        ArgumentList()),
                                                                IdentifierName(GetHttpRequestKeyword(parsedMethod.Declaration.HttpInfo))))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SeparatedList<ArgumentSyntax>(
                                                                    AddMethodParametersToRequest(
                                                                        parsedMethod,
                                                                        controllerDeclaration))))))))),
                            IdentifierName("Content")),
                        GenericName(
                                Identifier("ReadAsAsync"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SingletonSeparatedList<TypeSyntax>(
                                        IdentifierName($"{parsedMethod.Declaration.ReturnType}"))))))));
        // ExpressionStatement(
        //     AssignmentExpression(
        //         SyntaxKind.SimpleAssignmentExpression,
        //         IdentifierName("result"),
        //         CastExpression(
        //             IdentifierName($"{parsedMethod.Declaration.ReturnType}"),
        //             MemberAccessExpression(
        //                 SyntaxKind.SimpleMemberAccessExpression,
        //                 InvocationExpression(
        //                         MemberAccessExpression(
        //                             SyntaxKind.SimpleMemberAccessExpression,
        //                             MemberAccessExpression(
        //                                 SyntaxKind.SimpleMemberAccessExpression,
        //                                 ParenthesizedExpression(
        //                                     AwaitExpression(
        //                                         InvocationExpression(
        //                                                 MemberAccessExpression(
        //                                                     SyntaxKind.SimpleMemberAccessExpression,
        //                                                     ObjectCreationExpression(
        //                                                             QualifiedName(
        //                                                                 QualifiedName(
        //                                                                     QualifiedName(
        //                                                                         IdentifierName("System"),
        //                                                                         IdentifierName("Net")),
        //                                                                     IdentifierName("Http")),
        //                                                                 IdentifierName("HttpClient")))
        //                                                         .WithArgumentList(
        //                                                             ArgumentList()),
        //                                                     IdentifierName(
        //                                                         $"{parsedMethod.Declaration.HttpInfo.HttpType}Async")))
        //                                             .WithArgumentList(
        //                                                 ArgumentList(
        //                                                     SeparatedList<ArgumentSyntax>(
        //                                                         AddMethodParametersToRequest(
        //                                                             parsedMethod,
        //                                                             controllerDeclaration)))))),
        //                                 IdentifierName("Content")),
        //                             IdentifierName("ReadAsAsync")))
        //                     .WithArgumentList(
        //                         ArgumentList(
        //                             SingletonSeparatedList(
        //                                 Argument(
        //                                     TypeOfExpression(
        //                                         IdentifierName(
        //                                             $"{parsedMethod.Declaration.ReturnType}")))))),
        //                 IdentifierName("Result")))));
    }

    private InvocationExpressionSyntax ConsoleWriteLineExpression()
    {
        return InvocationExpression(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("Console"),
                IdentifierName("WriteLine")));
    }

    private InvocationExpressionSyntax ConsoleWriteLineWithContentsExpression(
        SyntaxList<InterpolatedStringContentSyntax> contents)
    {
        JavaParserException.ThrowIfNull(contents, nameof(contents));

        return ConsoleWriteLineExpression()
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(
                        Argument(
                            InterpolatedStringExpression(
                                    Token(SyntaxKind.InterpolatedStringStartToken))
                                .WithContents(contents)))));
    }

    private InterpolatedStringContentSyntax[] GetUriParams(ControllerDeclaration controllerDeclaration,
        ParsedMethod method)
    {
        JavaParserException.ThrowIfNull(controllerDeclaration, nameof(controllerDeclaration));
        JavaParserException.ThrowIfNull(method, nameof(method));

        return new InterpolatedStringContentSyntax[]
        {
            InterpolatedStringText()
                .WithTextToken(
                    Token(
                        TriviaList(),
                        SyntaxKind.InterpolatedStringTextToken,
                        $"Request URI: {_uri}:{_port}/{controllerDeclaration.BaseUri}" +
                        BuildRequestStringWithParams(method),
                        $"Request URI: {_uri}:{_port}/{controllerDeclaration.BaseUri}" +
                        BuildRequestStringWithParams(method),
                        TriviaList()))
        };
    }

    private string GetHttpRequestKeyword(HttpInfo httpInfo)
    {
        return httpInfo.HttpType.Contains("Get") || httpInfo.HttpType.Contains("Delete")
            ? $"{httpInfo.HttpType}Async"
            : $"{httpInfo.HttpType}AsJsonAsync";
    }

    private string BuildRequestStringWithParams(ParsedMethod method)
    {
        JavaParserException.ThrowIfNull(method, nameof(method));

        var builder = new StringBuilder();

        if (method.Declaration.HttpInfo.Uri != string.Empty)
            builder.Append($"{method.Declaration.HttpInfo.Uri}");

        foreach (var (param, httpType) in method.Declaration.Params)
        {
            if (httpType != "RequestParam") continue;

            builder.Append($"?{param.Name}={{{param.Name}}}");
        }

        return builder.ToString();
    }
}