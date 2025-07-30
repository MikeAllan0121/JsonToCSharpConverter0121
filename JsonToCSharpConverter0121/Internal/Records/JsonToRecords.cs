using JsonToCSharpConverter0121.Internal.Records.Naming;
using JsonToCSharpConverter0121.Internal.Records.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JsonToCSharpConverter0121.Internal.Records;

internal class JsonToRecords
{
    private JsonToRecords(RecordOptions options)
    {
        _options = options;
    }

    private readonly RecordOptions _options;
    private readonly Records _records = new();


    private IProperty CreateProperty(JsonProperty prop, NameBuilder nameBuilder) => GetProperty(prop, nameBuilder);

    private IProperty GetProperty(JsonProperty element, NameBuilder nameBuilder) => element.Value.ValueKind switch
    {
        JsonValueKind.String => new SimpleProperty(nameBuilder, "string"),
        JsonValueKind.Number => new SimpleProperty(nameBuilder, "double"),
        JsonValueKind.True or JsonValueKind.False => new SimpleProperty(nameBuilder, "bool"),
        JsonValueKind.Array => ElementIsAnArray(element, nameBuilder),
        JsonValueKind.Object => new ComplexProperty(nameBuilder, CreateRecord(element, nameBuilder)),
        _ => new DynamicProperty(nameBuilder)
    };

    private IProperty GetProperty(JsonElement element, NameBuilder nameBuilder, CollectionSetting isCollection) => element.ValueKind switch
    {
        JsonValueKind.String => new SimpleProperty(nameBuilder, "string", isCollection),
        JsonValueKind.Number => new SimpleProperty(nameBuilder, "double", isCollection),
        JsonValueKind.True or JsonValueKind.False => new SimpleProperty(nameBuilder, "bool"),
        _ => new DynamicProperty(nameBuilder)
    };

    private IProperty ElementIsAnArray(JsonProperty element, NameBuilder nameBuilder)
    {
        var arrayItems = element.Value.EnumerateArray();
        // TODO: fix this so it defo returns one
        var complex = arrayItems
           .Where(item => item.ValueKind == JsonValueKind.Object)
           .Select((obj, index) => CreateRecord(obj, nameBuilder))
           .ToList();
        // TODO: fix this so it defo returns one
        var simple = arrayItems
            .Where(item => item.ValueKind != JsonValueKind.Object)
            .Select(item => GetProperty(item, nameBuilder, CollectionSetting.IsCollection))
            .ToList();

        if (complex.Count > 0 && simple.Count > 0)
        {
            return new DynamicProperty(nameBuilder);
        }
        if(complex.Count > 0)
        {
            return new ComplexProperty(nameBuilder, complex.First(), CollectionSetting.IsCollection);
        }
        if(simple.Count > 0 && simple.All(s => s.PropertyType == simple.First().PropertyType))
        {
            return simple.First();
        }
        return new DynamicProperty(nameBuilder); 
    }
    private Record CreateRecord(JsonElement property, NameBuilder nameBuilder)
    {
        Record record = GenerateRecord(nameBuilder);

        foreach (var childProp in property.EnumerateObject())
        {
            GenerateChildProperty(record, childProp);
        }
        return record;
    }

    private Record CreateRecord(JsonProperty property, NameBuilder nameBuilder)
    {
        Record record = GenerateRecord(nameBuilder);

        foreach (var childProp in property.Value.EnumerateObject())
        {
            GenerateChildProperty(record, childProp);
        }
        return record;
    }

    private void GenerateChildProperty(Record record, JsonProperty childProp)
    {
        var nameBuilder = record.Name.ExtendNameBuilder(childProp.Name);
        var newProperty = CreateProperty(childProp, nameBuilder);
        var nameMatched = record.Properties.FirstOrDefault(p => p.Name == newProperty.Name);
        if (
            nameMatched is not null
            && (newProperty.GetType() != nameMatched.GetType()
                || (nameMatched is SimpleProperty nm && newProperty is SimpleProperty np && nm.Type != np.Type)))
        {
            record.Properties.Remove(nameMatched);
            record.Properties.Add(new DynamicProperty(nameMatched.Name, nameMatched.CollectionSetting));
            return;
        }
        if (!record.Properties.Contains(newProperty))
        {
            record.Properties.Add(newProperty);
        }
    }

    private Record GenerateRecord(NameBuilder nameBuilder)
    {
        _records.AddIfNew(nameBuilder);
        var record = _records[nameBuilder];
        return record;
    }

    public static IRecords Convert(JsonDocument jsonDocument, RecordOptions options)
    {
        var converter = new JsonToRecords(options);
        var root = jsonDocument.RootElement;
        NameBuilder nameBuilder = new(options.RootRecordName, options.TypeNamingConvention);
        if (root.ValueKind == JsonValueKind.Object)
        {
            converter.CreateRecord(root, nameBuilder);
        }
        if (root.ValueKind != JsonValueKind.Array)
        {
            return converter._records;
        }
        foreach (var property in root.EnumerateArray())
        {
            if (property.ValueKind == JsonValueKind.Object)
            {
                converter.CreateRecord(property, nameBuilder);
            }
        }
        return converter._records;
    }
}
