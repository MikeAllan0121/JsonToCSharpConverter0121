using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Extensions0121.Result0121;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121.Internal;

internal static class StringExtensions
{
    public static Result IsValidJson(this string json)
    {
        try
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            return Result.Ok();
        }
        catch (JsonException e)
        {
            return Result.Failed(e);
        }
    }

    public static string ConvertToCSharpPropertyName(this string name)
    {
        string cleanedName = new([.. name.Where(c => char.IsLetterOrDigit(c) || c == '_')]);

        cleanedName =  string.Join("", cleanedName
            .Split([' ', '_'], StringSplitOptions.RemoveEmptyEntries)
            .Select(word => char.ToUpper(word[0]) + word[1..]));
        return char.IsDigit(cleanedName[0])
            ? "_" + cleanedName 
            : cleanedName;
    }

    public static bool HasIllegalJsonChars(this string name)
        => string.IsNullOrEmpty(name)
        || !char.IsLetter(name[0]) 
        || name.Any(ch => !char.IsLetterOrDigit(ch));
}