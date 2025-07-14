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

public class JsonToCSharpConverter
{
    public static Result<string> CreateFullText(string json, Action<CSharpToJson0121Options>? configureOptions = null)
    {
        var options = new CSharpToJson0121Options();
        configureOptions?.Invoke(options);

        var result = json.IsValidJson();
        if (result.IsFailed)
        {
            return result.ToResult<string>();
        }

        var jsonToCSharpConvert = JsonToCSharpConvert.Create(json, options);

        return Result.Ok(string.Join($"{Environment.NewLine}{Environment.NewLine}", jsonToCSharpConvert.RecordsOrdered.Values)); 
    }

}
