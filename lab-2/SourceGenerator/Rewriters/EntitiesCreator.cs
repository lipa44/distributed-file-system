using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Entities;
using SourceGenerator.Exceptions;

namespace SourceGenerator.Rewriters;

public class EntitiesCreator : CSharpSyntaxRewriter
{
    private EntityDeclaration EntityDeclaration { get; set; }

    public EntitiesCreator WithEntity(EntityDeclaration entityDeclaration)
    {
        JavaParserException.ThrowIfNull(entityDeclaration, nameof(entityDeclaration));

        EntityDeclaration = entityDeclaration;

        return this;
    }

    public override SyntaxNode? VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        // 1. SyntaxTokenList - modifiers (access ones)
        var modifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        // 2. TypeSyntax - keyword (record)
        var keyword = SyntaxFactory.Token(SyntaxKind.RecordKeyword);

        // 3. SyntaxToken - identifier
        var identifier = SyntaxFactory.ParseToken(EntityDeclaration.Name);

        // 4. ParameterListSyntax - parameterList
        var parameterList = SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(
            EntityDeclaration.Params.Select(methodParam =>
                SyntaxFactory.Parameter(SyntaxFactory.ParseToken(methodParam.Name))
                    .WithType(SyntaxFactory.ParseTypeName(methodParam.Type)))));

        // 5. SyntaxToken? semiColonToken
        var semicolonToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);

        // RecordDeclaration creating
        var record = SyntaxFactory.RecordDeclaration(SyntaxKind.RecordDeclaration, keyword, identifier)
            .WithModifiers(modifiers)
            .WithParameterList(parameterList)
            .WithSemicolonToken(semicolonToken)
            .NormalizeWhitespace();

        return node.AddMembers(record).NormalizeWhitespace();
    }
}