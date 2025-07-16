# JSON to C# Converter

## Overview
A simple utility for converting JSON structures into C# record declarations. This tool helps developers generate strongly typed C# objects.

## Features
- Parses JSON and converts it into structured C# records.
- Supports customizable options for naming conventions and nullable properties.
- Handles JSON validation before conversion.

## Installation
To use this package, include it in your project:

```csharp
// Add the required using directives
using JsonToCSharpConverter0121;

```
## Usage
Basic Conversion
To convert JSON into C# records, use:

```csharp
string json = "{ \"name\": \"John\", \"age\": 30 }";
var result = JsonToCSharpConverter.CreateFullText(json);

if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}
```

## Configuration Options
You can customize how the conversion works using CSharpToJson0121Options


```csharp
var result = JsonToCSharpConverter.CreateFullText(json, options =>
{
    options.Nullable = Nullable.AllProperties;
    options.RootRecordName = "CustomRoot";
});
```

## Options Explained

| Option                | Description                                      |
|-----------------------|--------------------------------------------------|
| `Nullable`           | Specifies how nullable properties are handled    |
| `RootRecordName`     | Sets the name of the root record                 |
| `PropertyCreationMethod` | Determines how properties are generated (e.g., as positional parameters or init-only properties) and controls formatting such asmultiline layout based on property count |
| `ReadOnlyCollections` | Defines the collection type used for read-only properties (List or ReadOnlyCollection) |
| `TypeNamingConvention` | Specifies how type names should be generated (PropertyName or NestedPositionName) |


## Example Output
Given the JSON:

```json
{
  "name": "John",
  "age": 30
}
```

```csharp
public record Root(string Name, int Age);
```

## Contributing
Feel free to submit pull requests or report issues. Feedback is always welcome!

## License
MIT License. See LICENSE file for details.