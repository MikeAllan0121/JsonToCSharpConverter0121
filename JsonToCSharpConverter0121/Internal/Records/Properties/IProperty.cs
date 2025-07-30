using JsonToCSharpConverter0121.Internal.Records.Naming;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JsonToCSharpConverter0121.Internal.Records.Properties;

internal interface IProperty<T> : IProperty
{
    T Type { get; }
};

internal interface IProperty
{
    NameBuilder Name { get; }
    PropertyType PropertyType { get; }

    CollectionSetting CollectionSetting { get; }

    PropertyDeclarationSyntax CreateParameterInitAutoProperty(RecordCreationOptions options);
    ParameterSyntax CreateParameterPropertyByPositional(RecordCreationOptions options);
};