namespace JsonToCSharpConverter0121;

public class CSharpToJson0121Options
{
    private const string ROOT = "Root";
    public Nullable Nullable { get; set; }

    public PropertyCreationMethod PropertyCreationMethod { get; set; } = PropertyCreationMethod.PositionalParameters();

    public string RootRecordName { get; set; } = ROOT;

    public CollectionType ReadOnlyCollections { get; set; } = CollectionType.List;

    public TypeNamingConvention TypeNamingConvention { get; set; } = TypeNamingConvention.PropertyName;
}
