using Extensions0121.Results;
using JsonToCSharpConverter0121.Internal.Records;
using JsonToCSharpConverter0121.Internal.Records.Properties;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121.Internal;

internal class JsonToCSharpConvert
{
    private readonly CSharpToJson0121Options _options;
    private JsonToCSharpConvert(CSharpToJson0121Options options)
    {
        _options = options;
    }
    public static JsonToCSharpConvert Create(CSharpToJson0121Options options) => new(options);
    public Result<string> Convert(string json)
    {
        var options = new RecordOptions(_options.RootRecordName, _options.TypeNamingConvention);
        using var doc = JsonDocument.Parse(json);
        var result = JsonToRecords.Convert(doc, options);

        if (!IfPositionalParametersThenIsValid(result))
        {
            return Result.Ok<string>(ForceInitAutoProperties(doc), $"C# object requires JsonPropertyName. {nameof(PropertyCreationType)} cannot be {nameof(PropertyCreationType.PositionalParameters)}. Setting {nameof(PropertyCreationType)} to {nameof(PropertyCreationType.InitAutoProperties)}");
        }
        return GenerateCSharpText(result);
    }

    private string ForceInitAutoProperties(JsonDocument doc)
    {
        _options.PropertyCreationMethod = PropertyCreationMethod.InitAutoProperties();
        var fallbackOptions = new RecordOptions(_options.RootRecordName, _options.TypeNamingConvention);
        var fallbackResult = JsonToRecords.Convert(doc, fallbackOptions);
        return GenerateCSharpText(fallbackResult);
    }

    private bool IfPositionalParametersThenIsValid(IRecords result)
    {
        return !(_options.PropertyCreationMethod.Type == PropertyCreationType.PositionalParameters && AnyPropertyNeedsJsonPropertyName(result));
    }

    private static bool AnyPropertyNeedsJsonPropertyName(IRecords records) =>
        records.Items.SelectMany(r => r.Properties)
                 .Any(p => p.Name.Name.HasIllegalJsonChars());


    private string GenerateCSharpText(IRecords records)
    {
        RecordCreationOptions recordCreationOptions = new(_options.PropertyCreationMethod, _options.Nullable, _options.CollectionType);
        var recordsDeclaration = records.Items.Select(s => s.ToRecordDeclarationSyntax(recordCreationOptions))
            .ToList();
        return string.Join($"{Environment.NewLine}{Environment.NewLine}", recordsDeclaration.Select(r => r.ToFullString()));
    }
}
