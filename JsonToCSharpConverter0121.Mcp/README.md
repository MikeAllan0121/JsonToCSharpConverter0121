# JsonToCSharpConverter0121.Mcp

<!-- mcp-name: io.github.MikeAllan0121/JsonToCSharpConverter0121 -->

An MCP (stdio) server wrapping the JsonToCSharpConverter0121 library.

## Install

Requires the .NET 10 SDK (`dnx`). The package is stable, so no `--prerelease` flag is needed. The examples below pin version 3.1.0; you can omit the `@3.1.0` to get the latest stable release.

Claude Code:

```
claude mcp add json-to-csharp -- dnx JsonToCSharpConverter0121.Mcp@3.1.0 --yes
```

VS Code (`.vscode/mcp.json`):

```json
{
  "servers": {
    "json-to-csharp": {
      "type": "stdio",
      "command": "dnx",
      "args": ["JsonToCSharpConverter0121.Mcp@3.1.0", "--yes"]
    }
  }
}
```

Claude Desktop (`claude_desktop_config.json`):

```json
{
  "mcpServers": {
    "json-to-csharp": {
      "command": "dnx",
      "args": ["JsonToCSharpConverter0121.Mcp@3.1.0", "--yes"]
    }
  }
}
```

## Tool

`json_to_csharp(json, rootRecordName = "Root", nullable = Nothing, propertyCreationType = PositionalParameters, newLineAfterProperties = 5, collectionType = List, typeNamingConvention = PropertyName)`
returns C# record definitions for the given JSON, or an error if the JSON is invalid.

## Configuration (from source)

```json
{
  "mcpServers": {
    "json-to-csharp": {
      "command": "dotnet",
      "args": ["run", "--project", "path/to/JsonToCSharpConverter0121.Mcp"]
    }
  }
}
```
