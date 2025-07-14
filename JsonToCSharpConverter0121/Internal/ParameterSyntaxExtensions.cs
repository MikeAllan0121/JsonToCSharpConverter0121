using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121.Internal;

internal static class ParameterSyntaxExtensions
{
    public static ParameterSyntax AddJsonPropertyAttributeIfRequired(
        this ParameterSyntax parameterSyntax, 
        string ogJsonName)
    {
        if (!ogJsonName.HasIllegalJsonChars())
        {
            return parameterSyntax;
        }
        var jsonPropertyAttr = SyntaxFactory.Attribute(SyntaxFactory.ParseName("JsonProperty"))
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
        return parameterSyntax.AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(jsonPropertyAttr)));
    }

    public static PropertyDeclarationSyntax AddJsonPropertyAttributeIfRequired(
        this PropertyDeclarationSyntax propertySyntax,
        string ogJsonName)
    {
        if (!ogJsonName.HasIllegalJsonChars())
        {
            return propertySyntax;
        }

        var jsonPropertyAttr = SyntaxFactory.Attribute(SyntaxFactory.ParseName("JsonProperty"))
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

        return propertySyntax.AddAttributeLists(
            SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(jsonPropertyAttr))
        );
    }

}
