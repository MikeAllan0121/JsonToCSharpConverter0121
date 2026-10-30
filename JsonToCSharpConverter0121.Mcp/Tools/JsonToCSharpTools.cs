using JsonToCSharpConverter0121;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace JsonToCSharpConverter0121.Mcp.Tools;

/// <summary>
/// MCP tools that expose the JsonToCSharpConverter0121 library.
/// </summary>
[McpServerToolType]
public sealed class JsonToCSharpTools
{
    [McpServerTool(Name = "json_to_csharp", ReadOnly = true, Idempotent = true)]
    [Description("Converts a JSON document into C# record type definitions and returns the generated C# code.")]
    public static string JsonToCSharp(
        [Description("The JSON text to convert.")] string json,
        [Description("Name of the root record. Defaults to 'Root'.")] string rootRecordName = "Root",
        [Description("Which properties are nullable: Nothing, AllPropertiesExceptObjectsAndCollections, AllPropertiesExceptCollections or AllProperties.")] JsonToCSharpConverter0121.Nullable nullable = JsonToCSharpConverter0121.Nullable.Nothing,
        [Description("How properties are declared: PositionalParameters or InitAutoProperties.")] PropertyCreationType propertyCreationType = PropertyCreationType.PositionalParameters,
        [Description("For PositionalParameters only: put each parameter on its own line once a record has at least this many properties. Use 0 or less to keep everything on one line.")] int newLineAfterProperties = 5,
        [Description("Collection type for arrays: List, IList, IEnumerable or IReadOnlyCollection.")] CollectionType collectionType = CollectionType.List,
        [Description("How nested record types are named: PropertyName or NestedPositionName.")] TypeNamingConvention typeNamingConvention = TypeNamingConvention.PropertyName)
    {
        PropertyCreationMethod method = propertyCreationType == PropertyCreationType.InitAutoProperties
            ? PropertyCreationMethod.InitAutoProperties()
            : PropertyCreationMethod.PositionalParameters(
                newLineAfterProperties > 0 ? HavePropertiesOnNewLine.Yes(newLineAfterProperties) : HavePropertiesOnNewLine.No());

        CSharpToJson0121Options options = new()
        {
            RootRecordName = rootRecordName,
            Nullable = nullable,
            PropertyCreationMethod = method,
            CollectionType = collectionType,
            TypeNamingConvention = typeNamingConvention,
        };

        var result = JsonToCSharpConverter.CreateFullText(json, options);
        if (result.IsFailed)
        {
            throw new McpException($"Conversion failed: {result.ErrorMessage}");
        }

        return result.Value;
    }
}
