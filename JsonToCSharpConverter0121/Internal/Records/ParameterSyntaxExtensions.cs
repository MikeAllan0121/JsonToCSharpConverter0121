using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121.Internal.Records;

internal static class ParameterSyntaxExtensions
{
    public static ParameterSyntax AddJsonPropertyAttributeIfRequired(
        this ParameterSyntax parameterSyntax, 
        string ogJsonName)
    {
        var attr = CreateJsonPropertyAttributeIfRequired(ogJsonName);
        return attr is null ? parameterSyntax : parameterSyntax.AddAttributeLists(attr);
    }

    public static PropertyDeclarationSyntax AddJsonPropertyAttributeIfRequired(
        this PropertyDeclarationSyntax propertySyntax,
        string ogJsonName)
    {
        var attr = CreateJsonPropertyAttributeIfRequired(ogJsonName);
        return attr is null ? propertySyntax : propertySyntax.AddAttributeLists(attr);
    }

    private static AttributeListSyntax? CreateJsonPropertyAttributeIfRequired(string ogJsonName)
    {
        if (!ogJsonName.HasIllegalJsonChars())
            return null;

        var jsonPropertyAttr = SyntaxFactory.Attribute(SyntaxFactory.ParseName("JsonPropertyName"))
            .WithArgumentList(
                SyntaxFactory.AttributeArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Literal(ogJsonName)
                            )
                        )
                    )
                )
            );
        return SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(jsonPropertyAttr));
    }

}
