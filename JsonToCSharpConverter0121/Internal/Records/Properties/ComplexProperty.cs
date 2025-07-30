using Extensions0121.Exceptions;
using JsonToCSharpConverter0121.Internal.Records.Naming;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Specialized;

namespace JsonToCSharpConverter0121.Internal.Records.Properties;

internal record ComplexProperty(NameBuilder Name, Record Type, CollectionSetting CollectionSetting = CollectionSetting.NotACollection) : IProperty<Record>
{
    public PropertyType PropertyType => PropertyType.Complex;

    public PropertyDeclarationSyntax CreateParameterInitAutoProperty(RecordCreationOptions options)
    {
        string fullType = GetFullType(options);
        return this.CreateParameterInitAutoProperty(fullType);
    }

    public ParameterSyntax CreateParameterPropertyByPositional(RecordCreationOptions options)
    {
        string fullType = GetFullType(options);
        return this.CreateParameterPropertyByPositional(fullType);
    }

    private string GetFullType(RecordCreationOptions options)
    {
        return CollectionSetting switch
        {
            CollectionSetting.NotACollection => options.Nullable switch
            {
                Nullable.Nothing or
                Nullable.AllPropertiesExceptObjectsAndCollections => Type.TypeName,
                Nullable.AllPropertiesExceptCollections or
                Nullable.AllProperties => $"{Type.Name.FullCsharpName}?",
                _ => throw ThrowHelper.EnumExhausted(nameof(Nullable)),
            },
            CollectionSetting.IsCollection => options.Nullable switch
            {
                Nullable.Nothing or
                Nullable.AllPropertiesExceptObjectsAndCollections or
                Nullable.AllPropertiesExceptCollections => options.CollectionType.CollectionType(Type.TypeName),
                Nullable.AllProperties => $"{options.CollectionType.CollectionType(Type.TypeName)}?",
                _ => throw ThrowHelper.EnumExhausted(nameof(Nullable)),
            },
            _ => throw ThrowHelper.EnumExhausted(nameof(CollectionSetting)),
        };
    }
}
