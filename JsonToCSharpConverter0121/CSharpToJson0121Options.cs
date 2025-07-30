using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JsonToCSharpConverter0121.Tests")]
namespace JsonToCSharpConverter0121;
/// <summary>
/// Options for controlling the conversion of JSON to C# record types.
/// </summary>
public class CSharpToJson0121Options
{
    private const string ROOT = "Root";

    /// <summary>
    /// Specifies the nullability behavior for generated properties.
    /// Controls which properties are marked as nullable in the generated C# code.
    /// </summary>
    public Nullable Nullable { get; set; }

    /// <summary>
    /// Determines how properties are created in the generated record types.
    /// Use <see cref="PropertyCreationMethod.PositionalParameters"/> for positional parameters
    /// or <see cref="PropertyCreationMethod.InitAutoProperties"/> for init-only auto-properties.
    /// </summary>
    public PropertyCreationMethod PropertyCreationMethod { get; set; } = PropertyCreationMethod.PositionalParameters();

    /// <summary>
    /// The name to use for the root record type in the generated C# code.
    /// Defaults to "Root".
    /// </summary>
    public string RootRecordName { get; set; } = ROOT;

    /// <summary>
    /// Specifies the collection type to use for array properties in the generated code.
    /// For example, <see cref="CollectionType.IList"/> or <see cref="CollectionType.IReadOnlyCollection"/>.
    /// </summary>
    public CollectionType CollectionType { get; set; } = CollectionType.List;

    /// <summary>
    /// Determines the naming convention for generated record types.
    /// For example, <see cref="TypeNamingConvention.PropertyName"/> or <see cref="TypeNamingConvention.NestedPositionName"/>.
    /// </summary>
    public TypeNamingConvention TypeNamingConvention { get; set; } = TypeNamingConvention.PropertyName;
}
