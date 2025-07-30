using JsonToCSharpConverter0121.Internal.Records;
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
    public string Convert(string json)
    {
        var options = new RecordOptions(_options.RootRecordName, _options.TypeNamingConvention);
        using var doc = JsonDocument.Parse(json);
        var result = JsonToRecords.Convert(doc, options);
        RecordCreationOptions recordCreationOptions = new(_options.PropertyCreationMethod, _options.Nullable, _options.CollectionType);
        var recordsDeclaration = result.Items.Select(s => s.ToRecordDeclarationSyntax(recordCreationOptions))
            .ToList();
        return string.Join($"{Environment.NewLine}{Environment.NewLine}", recordsDeclaration.Select(r => r.ToFullString()));
    }
}
