namespace JsonToCSharpConverter0121;

public class CSharpToJson0121Options
{
    private const string ROOT = "Root";
    public Nullable Nullable { get; set; }

    public HavePropertiesOnNewLine HavePropertiesOnNewLine { get; set; } = HavePropertiesOnNewLine.Yes();

    public string RootRecordName { get; set; } = ROOT;

    public CollectionType ReadOnlyCollections { get; set; } = CollectionType.List;

    public TypeNamingConvention TypeNamingConvention { get; set; } = TypeNamingConvention.PropertyName;
}
