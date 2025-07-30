using Extensions0121.Exceptions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JsonToCSharpConverter0121.Internal.Records.Properties;

internal static class IPropertyExtensions
{
    public static PropertyDeclarationSyntax CreateParameterInitAutoForSimpleProperty(this IProperty<string> property, RecordCreationOptions options)
    {
        string fullType = property.GetFullType(options);

        return property.CreateParameterInitAutoProperty(fullType);
    }

    public static ParameterSyntax CreateParameterForSimplePropertyByPositional(this IProperty<string> property, RecordCreationOptions options)
    {
        string fullType = property.GetFullType(options);

        return property.CreateParameterPropertyByPositional(fullType);
    }

    private static string GetFullType(this IProperty<string> property, RecordCreationOptions options)
    {
        return property.CollectionSetting switch
        {
            CollectionSetting.NotACollection => options.Nullable switch
            {
                Nullable.Nothing => property.Type,
                Nullable.AllPropertiesExceptObjectsAndCollections or
                Nullable.AllPropertiesExceptCollections or
                Nullable.AllProperties => $"{property.Type}?",
                _ => throw ThrowHelper.EnumExhausted(nameof(Nullable)),
            },
            CollectionSetting.IsCollection => options.Nullable switch
            {
                Nullable.Nothing or
                Nullable.AllPropertiesExceptObjectsAndCollections or
                Nullable.AllPropertiesExceptCollections => options.CollectionType.CollectionType(property.Type),
                Nullable.AllProperties => $"{options.CollectionType.CollectionType(property.Type)}?",
                _ => throw ThrowHelper.EnumExhausted(nameof(Nullable)),
            },
            _ => throw ThrowHelper.EnumExhausted(nameof(CollectionSetting)),
        };
    }

    public static PropertyDeclarationSyntax CreateParameterInitAutoProperty(this IProperty property, string fullType)
    {
        var propertyType = SyntaxFactory.ParseTypeName(fullType);

        return SyntaxFactory.PropertyDeclaration(propertyType, property.Name.CsharpName)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .WithAccessorList(
                SyntaxFactory.AccessorList(
                    SyntaxFactory.List(
                    [
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.InitAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    ])
                )
            )
            .AddJsonPropertyAttributeIfRequired(property.Name.Name);
    }

    public static ParameterSyntax CreateParameterPropertyByPositional(this IProperty property, string fullType)
    {
        return SyntaxFactory.Parameter(SyntaxFactory.Identifier(property.Name.CsharpName))
            .WithType(SyntaxFactory.ParseTypeName(fullType))
            .AddJsonPropertyAttributeIfRequired(property.Name.Name);
    }

}
