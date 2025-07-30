using Extensions0121.Exceptions;
using JsonToCSharpConverter0121.Internal.Records.Naming;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JsonToCSharpConverter0121.Internal.Records.Properties;

internal record DynamicProperty(NameBuilder Name, CollectionSetting CollectionSetting = CollectionSetting.NotACollection) : IProperty<string>
{
    public PropertyType PropertyType => PropertyType.Dynamic;
    public string Type => "object";

    public PropertyDeclarationSyntax CreateParameterInitAutoProperty(RecordCreationOptions options) => this.CreateParameterInitAutoForSimpleProperty(options);

    public ParameterSyntax CreateParameterPropertyByPositional(RecordCreationOptions options) => this.CreateParameterForSimplePropertyByPositional(options);
}
