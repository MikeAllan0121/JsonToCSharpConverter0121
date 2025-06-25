using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121.Internal;

internal static class CollectionTypeExtensions
{
    public static string StringName(this JsonToCSharpConverter0121.CollectionType collectionType) => collectionType switch
    {
        JsonToCSharpConverter0121.CollectionType.List => "List",
        JsonToCSharpConverter0121.CollectionType.ReadOnlyCollection => "ReadOnlyCollection",
        _ => throw new NotImplementedException($"Enum of type {nameof(JsonToCSharpConverter0121.CollectionType)} exhausted"),
    };
}
