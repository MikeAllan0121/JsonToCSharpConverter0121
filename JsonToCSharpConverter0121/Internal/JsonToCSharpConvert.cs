using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using Newtonsoft.Json;
using Extensions0121.Exceptions;

namespace JsonToCSharpConverter0121.Internal;

internal class JsonToCSharpConvert
{
    private readonly CSharpToJson0121Options _options;
    private readonly Dictionary<string, RecordDeclarationSyntax> _records = [];
    private readonly Lazy<Dictionary<string, RecordDeclarationSyntax>> _recordsOrdered;

    public IReadOnlyDictionary<string, RecordDeclarationSyntax> NameRecordDeclarations => _records;
    public IReadOnlyCollection<RecordDeclarationSyntax> RecordDeclarations => _records.Values;
    public IReadOnlyDictionary<string, RecordDeclarationSyntax> RecordsOrdered => _recordsOrdered.Value; 
    private JsonToCSharpConvert(CSharpToJson0121Options cSharpToJson0121Options)
    {
        _options = cSharpToJson0121Options;
        _recordsOrdered = new(() => _records.Reverse().ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
    }

    public static JsonToCSharpConvert Create(string jsonString, CSharpToJson0121Options options)
    {
        var convert = new JsonToCSharpConvert(options);

        var jsonDoc = JsonDocument.Parse(jsonString);
        string rootRecordName = options.RootRecordName;

        convert.ProcessJsonObject(jsonDoc.RootElement, rootRecordName);
        return convert;
    }

    private void ProcessJsonObject(JsonElement element, string recordName)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        RecordDeclarationSyntax record = _options.PropertyCreationMethod.Type switch
        {
            PropertyCreationType.PositionalParameters => PropertiesByPositionalParameters(element, recordName),
            PropertyCreationType.InitAutoProperties => InitAutoPropertiesParameters(element, recordName),
            _ => throw ThrowHelper.EnumExhausted(nameof(PropertyCreationType)),
        };

        _records[recordName] = record;
    }

    private RecordDeclarationSyntax InitAutoPropertiesParameters(JsonElement element, string recordName)
    {
        var propertyDeclarations = element.EnumerateObject().Select(e => CreateParameterInitAutoProperty(e, recordName));

        var record = SyntaxFactory.RecordDeclaration(SyntaxFactory.Token(SyntaxKind.RecordKeyword), SyntaxFactory.Identifier(recordName))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
            .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(propertyDeclarations))
            .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken))
            .NormalizeWhitespace(elasticTrivia: true);

        return record;
    }

    private RecordDeclarationSyntax PropertiesByPositionalParameters(JsonElement element, string recordName)
    {

        var propertyCount = element.GetPropertyCount();

        var properties = element.EnumerateObject().Select(e => CreateParameterPropertyByPositional(e, recordName));

        var separatedList = _options.PropertyCreationMethod.HavePropertiesOnNewLine.DoesNotRequiresNewLine(propertyCount)
            ? SyntaxFactory.SeparatedList(properties.Select(p => p.NormalizeWhitespace()))
            : SyntaxFactory.SeparatedList<ParameterSyntax>(
                properties.Select((prop, index) =>
                    new SyntaxNodeOrToken[]
                    {
                        prop.NormalizeWhitespace().WithLeadingTrivia(SyntaxFactory.ElasticWhitespace("    ")),
                        index < propertyCount - 1
                            ? SyntaxFactory.Token(new(SyntaxFactory.ElasticMarker), SyntaxKind.CommaToken, SyntaxFactory.TriviaList(SyntaxFactory.ElasticCarriageReturnLineFeed, SyntaxFactory.ElasticMarker))
                            : null // Prevent trailing comma
                    }
                ).SelectMany(x => x).Where(x => x != null));

        var parameterList = _options.PropertyCreationMethod.HavePropertiesOnNewLine.DoesNotRequiresNewLine(propertyCount)
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
        var record = SyntaxFactory.RecordDeclaration(SyntaxFactory.Token(SyntaxKind.RecordKeyword), SyntaxFactory.Identifier(recordName))
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .NormalizeWhitespace(elasticTrivia: true)
            .WithParameterList(parameterList)
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return record;
    }

    private static string GetCSharpType(JsonElement element) => element.ValueKind switch
    {
        JsonValueKind.String => "string",
        JsonValueKind.Number => "double",
        JsonValueKind.True or JsonValueKind.False => "bool",
        JsonValueKind.Array => throw new Exception("Only basic can be handled"),
        JsonValueKind.Object => throw new Exception("Only basic can be handled"),
        _ => "object"
    };

    private ParameterSyntax CreateParameterPropertyByPositional(JsonProperty prop, string typePath)
    {
        var propertyTypeName = prop.Name.ConvertToCSharpPropertyName();

        string typeName = GetCSharpType(prop, propertyTypeName, typePath);
        return SyntaxFactory.Parameter(SyntaxFactory.Identifier(propertyTypeName))
            .WithType(SyntaxFactory.ParseTypeName($"{typeName}{_options.Nullable.GetNullabilitySuffix(prop.Value)}"))
            .AddJsonPropertyAttributeIfRequired(prop.Name);
    }

    private PropertyDeclarationSyntax CreateParameterInitAutoProperty(JsonProperty prop, string typePath)
    {
        var propertyTypeName = prop.Name.ConvertToCSharpPropertyName();
        string typeName = GetCSharpType(prop, propertyTypeName, typePath);

        var propertyType = SyntaxFactory.ParseTypeName($"{typeName}{_options.Nullable.GetNullabilitySuffix(prop.Value)}");

        var property = SyntaxFactory.PropertyDeclaration(propertyType, propertyTypeName)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .WithAccessorList(
                SyntaxFactory.AccessorList(
                    SyntaxFactory.List(new[]
                    {
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.InitAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    })
                )
            )
            .AddJsonPropertyAttributeIfRequired(prop.Name);

        return property;
    }


    private string GetCSharpType(JsonProperty element, string propertyTypeName, string typePath) => element.Value.ValueKind switch
    {
        JsonValueKind.String => "string",
        JsonValueKind.Number => "double",
        JsonValueKind.True or JsonValueKind.False => "bool",
        JsonValueKind.Array => $"{_options.ReadOnlyCollections.StringName()}<{InferCollectionType(element, propertyTypeName, typePath)}>",
        JsonValueKind.Object => InferNestedRecordType(element, propertyTypeName, typePath),
        _ => "object"
    };

    private string InferNestedRecordType(JsonProperty property, string propertyTypeName, string typePath)
    {
        var fullpropertyName = (_options.TypeNamingConvention == TypeNamingConvention.PropertyName ? "" : typePath) + propertyTypeName;
        if (!_records.ContainsKey(fullpropertyName))
        {
            ProcessJsonObject(property.Value, fullpropertyName);
        }
        return fullpropertyName;
    }

    private string InferCollectionType(JsonProperty property, string propertyTypeName, string typePath)
    {
        var fullpropertyName = (_options.TypeNamingConvention == TypeNamingConvention.PropertyName ? "" : typePath) + propertyTypeName;
        var elements = property.Value.EnumerateArray().ToArray();

        if (elements.Length == 0)
        {
            return "object"; // Handle empty arrays properly
        }

        var firstElement = elements.FirstOrDefault();

        if (firstElement.ValueKind == JsonValueKind.Object)
        {
            if (!_records.ContainsKey(fullpropertyName))
            {
                ProcessJsonObject(firstElement, fullpropertyName);
            }
            return fullpropertyName; // Return the inferred record name instead of drilling into properties
        }

        return GetCSharpType(firstElement);
    }
}
