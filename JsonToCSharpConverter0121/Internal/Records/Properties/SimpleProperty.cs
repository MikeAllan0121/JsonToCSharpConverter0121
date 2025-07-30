using Extensions0121.Exceptions;
using JsonToCSharpConverter0121.Internal.Records.Naming;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection.Metadata;

namespace JsonToCSharpConverter0121.Internal.Records.Properties;

internal record SimpleProperty(NameBuilder Name, string Type, CollectionSetting CollectionSetting = CollectionSetting.NotACollection) : IProperty<string>
{
    public PropertyType PropertyType => PropertyType.Simple;

    public PropertyDeclarationSyntax CreateParameterInitAutoProperty(RecordCreationOptions options) => this.CreateParameterInitAutoForSimpleProperty(options);
    public ParameterSyntax CreateParameterPropertyByPositional(RecordCreationOptions options) => this.CreateParameterForSimplePropertyByPositional(options);
}
