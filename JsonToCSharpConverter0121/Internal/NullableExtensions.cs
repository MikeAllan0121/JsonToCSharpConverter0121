using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121.Internal;

internal static class NullableExtensions
{
    private const string QUESTION_MARK = "?";

    public static string GetNullabilitySuffix(this Nullable nullable, JsonElement element) => nullable switch
    {
        Nullable.Nothing => string.Empty,
        Nullable.AllPropertiesExceptObjectsAndCollections => element.ValueKind == JsonValueKind.Object || element.ValueKind == JsonValueKind.Array ? string.Empty : QUESTION_MARK,
        Nullable.AllPropertiesExceptCollections => element.ValueKind == JsonValueKind.Array ? string.Empty : QUESTION_MARK,
        Nullable.AllProperties => "?",
        _ => throw new NotImplementedException(),
    };
}
