# JsonToCSharpConverter0121

Converts JSON into strongly typed C# `record` declarations.

- Parses and validates JSON, then generates formatted C# records.
- Infers `int` for whole numbers and `double` for fractional numbers.
- Configurable nullability, property style, collection type, root name and type naming.

## Install

```
dotnet add package JsonToCSharpConverter0121
```

## Usage

```csharp
using JsonToCSharpConverter0121;

string json = """{ "name": "John", "age": 30, "height": 1.82 }""";

var result = JsonToCSharpConverter.CreateFullText(json);

if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}
```

Output:

```csharp
public sealed record Root(string Name, int Age, double Height);
```

### Options

```csharp
var result = JsonToCSharpConverter.CreateFullText(json, options =>
{
    options.RootRecordName = "Person";
    options.Nullable = JsonToCSharpConverter0121.Nullable.AllProperties;
    options.PropertyCreationMethod = PropertyCreationMethod.InitAutoProperties();
});
```

| Option | Description |
|---|---|
| `RootRecordName` | Name of the root record |
| `Nullable` | How nullable properties are handled |
| `PropertyCreationMethod` | Positional parameters or init-only properties, plus layout |
| `CollectionType` | Collection type used for JSON arrays |
| `TypeNamingConvention` | How nested type names are generated |

An MCP server wrapping this library is available as `JsonToCSharpConverter0121.Mcp`.

Source: https://github.com/MikeAllan0121/JsonToCSharpConverter0121 (MIT)
