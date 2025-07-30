using Extensions0121.Exceptions;

namespace JsonToCSharpConverter0121.Internal.Records;

public static class CollectionTypeExtensions
{
    public static string CollectionType(this CollectionType collectionSetting, string ofType) => $"{collectionSetting.CollectionType()}<{ofType}>";

    private static string CollectionType(this CollectionType collectionSetting) => collectionSetting switch
    {
        JsonToCSharpConverter0121.CollectionType.List => "List",
        JsonToCSharpConverter0121.CollectionType.IList => $"IList",
        JsonToCSharpConverter0121.CollectionType.IEnumerable => "IEnumerable",
        JsonToCSharpConverter0121.CollectionType.IReadOnlyCollection => $"IReadOnlyCollection",
        _ => throw ThrowHelper.EnumExhausted(nameof(CollectionType)),
    };

}