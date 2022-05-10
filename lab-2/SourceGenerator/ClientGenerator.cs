using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Helpers;
using SourceGenerator.Parsers;
using SourceGenerator.Rewriters;

namespace SourceGenerator;

[Generator]
public class ClientGenerator : ISourceGenerator
{
    public const string BasePort = "7388";
    public const string BaseUri = "http://localhost";
    public const string JavaApiDirectory = "/Users/lipa/Programming/IntelliJ Projects/techSpringCrud";

    private static readonly UsingDirectiveSyntax SystemDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
    private static readonly UsingDirectiveSyntax SystemGenericDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic"));
    private static readonly UsingDirectiveSyntax StringContentDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Net.Http"));
    private static readonly UsingDirectiveSyntax NewtonSoftJsonDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Newtonsoft.Json"));
    private static readonly UsingDirectiveSyntax HttpClientExtensionsDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Net.Http.Json"));

    public void Execute(GeneratorExecutionContext context)
    {
        // Java parser initializing
        var javaFilesDirectory = new FileInfo(JavaApiDirectory);
        var parser = new JavaParser(new BracketsChecker()).CalculateFilesToParse(javaFilesDirectory);

        var controllers = parser.ParseControllerDeclarations(parser.ControllerFiles.ToList());
        var entities = parser.ParseEntityDeclarations(parser.EntityFiles.ToList());

        // Source generator initializing
        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("LipaWebClient"));
        var directives = SyntaxFactory.List(new[] {SystemDirective, SystemGenericDirective, StringContentDirective, NewtonSoftJsonDirective, HttpClientExtensionsDirective});
        var modifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword));

        var compilationUnit = SyntaxFactory.CompilationUnit().WithUsings(directives);

        var methodsCreator = new MethodsCreator(BasePort);
        foreach (var controller in controllers)
        {
            var webClientClass = SyntaxFactory.ClassDeclaration($"{controller.Name}Api")
                .WithModifiers(modifiers);

            webClientClass = (ClassDeclarationSyntax) methodsCreator
                .WithController(controller)
                .VisitClassDeclaration(webClientClass)!;

            var controllerApi = compilationUnit
                .AddMembers(namespaceDeclaration
                    .AddMembers(webClientClass))
                .NormalizeWhitespace();

            context.AddSource($"{controller.Name}.g.cs", controllerApi.ToString());
        }

        var entitiesCreator = new EntitiesCreator();
        foreach (var entity in entities)
        {
            var namespaceRecord = (NamespaceDeclarationSyntax) entitiesCreator
                .WithEntity(entity)
                .VisitNamespaceDeclaration(namespaceDeclaration)!;

            var entityClass = compilationUnit
                .AddMembers(namespaceRecord)
                .NormalizeWhitespace();

            context.AddSource($"{entity.Name}.g.cs", entityClass.NormalizeWhitespace().ToString());
        }
    }

    public void Initialize(GeneratorInitializationContext context) { }
}