using Extensions0121.Exceptions;
using JsonToCSharpConverter0121.Internal.Records.Naming;
using JsonToCSharpConverter0121.Internal.Records.Properties;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;

namespace JsonToCSharpConverter0121.Internal.Records;

internal record Record(NameBuilder Name)
{
    public List<IProperty> Properties { get; } = [];

    public string TypeName => Name.FullCsharpName;

    public RecordDeclarationSyntax ToRecordDeclarationSyntax(RecordCreationOptions options) => options.PropertyCreationMethod.Type switch
        {
            PropertyCreationType.PositionalParameters => PropertiesByPositionalParameters(options),
            PropertyCreationType.InitAutoProperties => InitAutoPropertiesParameters(options),
            _ => throw ThrowHelper.EnumExhausted(nameof(PropertyCreationType)),
        };

    private RecordDeclarationSyntax InitAutoPropertiesParameters(RecordCreationOptions options)
    {
        var propertyDeclarations = Properties.Select(e => e.CreateParameterInitAutoProperty(options));
        return SyntaxFactory.RecordDeclaration(SyntaxFactory.Token(SyntaxKind.RecordKeyword), SyntaxFactory.Identifier(Name.FullCsharpName))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
            .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(propertyDeclarations))
            .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken))
            .NormalizeWhitespace(elasticTrivia: true);
    }

    private RecordDeclarationSyntax PropertiesByPositionalParameters(RecordCreationOptions options)
    {
        var propertyDeclarations = Properties.Select(e => e.CreateParameterPropertyByPositional(options));

        var separatedList = options.PropertyCreationMethod.HavePropertiesOnNewLine.DoesNotRequiresNewLine(Properties.Count)
            ? SyntaxFactory.SeparatedList(propertyDeclarations.Select(p => p.NormalizeWhitespace()))
            : SyntaxFactory.SeparatedList<ParameterSyntax>(
                propertyDeclarations.Select((prop, index) =>
                    new SyntaxNodeOrToken[]
                    {
                        prop.NormalizeWhitespace().WithLeadingTrivia(SyntaxFactory.ElasticWhitespace("    ")),
                        index < Properties.Count - 1
                            ? SyntaxFactory.Token(new(SyntaxFactory.ElasticMarker), SyntaxKind.CommaToken, SyntaxFactory.TriviaList(SyntaxFactory.ElasticCarriageReturnLineFeed, SyntaxFactory.ElasticMarker))
                            : null // Prevent trailing comma
                    }
                ).SelectMany(x => x).Where(x => x != null));

        var parameterList = options.PropertyCreationMethod.HavePropertiesOnNewLine.DoesNotRequiresNewLine(Properties.Count)
            ? SyntaxFactory.ParameterList(separatedList).NormalizeWhitespace()
                .WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                .WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken))
            : SyntaxFactory.ParameterList(separatedList)
                .WithOpenParenToken(
                    SyntaxFactory.Token(SyntaxKind.OpenParenToken).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
                )
                .WithCloseParenToken(
                    SyntaxFactory.Token(SyntaxKind.CloseParenToken).WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed)
                );
        return SyntaxFactory.RecordDeclaration(SyntaxFactory.Token(SyntaxKind.RecordKeyword), SyntaxFactory.Identifier(Name.FullCsharpName))
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .NormalizeWhitespace(elasticTrivia: true)
            .WithParameterList(parameterList)
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }
}
