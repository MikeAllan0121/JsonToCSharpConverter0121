using Extensions0121.Result0121;
using JsonToCSharpConverter0121.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Host;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121;

/// <summary>
/// Provides methods to convert JSON strings into C# record type definitions.
/// </summary>
public class JsonToCSharpConverter
{
    /// <summary>
    /// Converts a JSON string to C# record type definitions as a single formatted string.
    /// </summary>
    /// <param name="json">The JSON string to convert.</param>
    /// <param name="configureOptions">
    /// An optional delegate to configure <see cref="CSharpToJson0121Options"/> for controlling output,
    /// such as nullability, property creation style, collection type, root record name, and naming convention.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the generated C# code as a string if successful,
    /// or a failure result if the input JSON is invalid.
    /// </returns>
    public static Result<string> CreateFullText(string json, Action<CSharpToJson0121Options>? configureOptions = null)
    {
        var options = new CSharpToJson0121Options();
        configureOptions?.Invoke(options);
        return CreateFullText(json, options);
    }

    /// <summary>
    /// Converts a JSON string to C# record type definitions as a single formatted string.
    /// </summary>
    /// <param name="json">The JSON string to convert.</param>
    /// <param name="options">
    /// Options to configure <see cref="CSharpToJson0121Options"/> for controlling output,
    /// such as nullability, property creation style, collection type, root record name, and naming convention.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the generated C# code as a string if successful,
    /// or a failure result if the input JSON is invalid.
    /// </returns>
    public static Result<string> CreateFullText(string json, CSharpToJson0121Options options)
    {
        var result = json.IsValidJson();
        if (result.IsFailed)
        {
            return result.ToResult<string>();
        }

        var jsonToCSharpConvert = JsonToCSharpConvert.Create(options)
            .Convert(json);

        return Result.Ok(jsonToCSharpConvert);
    }
}
