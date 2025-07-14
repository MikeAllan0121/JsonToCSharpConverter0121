namespace JsonToCSharpConverter0121;

public class PropertyCreationMethod
{
    public PropertyCreationType Type { get; private set; }

    private readonly HavePropertiesOnNewLine? _havePropertiesOnNewLine;
    public HavePropertiesOnNewLine HavePropertiesOnNewLine =>
        Type == PropertyCreationType.PositionalParameters
        ? _havePropertiesOnNewLine ?? throw new Exception("HavePropertiesOnNewLine is not set for PositionalParameters")
        : throw new Exception("Type not valid for this property to be set");

    private PropertyCreationMethod(PropertyCreationType type, HavePropertiesOnNewLine? havePropertiesOnNewLine)
    {
        Type = type;
        _havePropertiesOnNewLine = havePropertiesOnNewLine;
    }

    public static PropertyCreationMethod PositionalParameters(HavePropertiesOnNewLine? havePropertiesOnNewLine = null)
    {
        havePropertiesOnNewLine ??= HavePropertiesOnNewLine.Yes();
        return new PropertyCreationMethod(PropertyCreationType.PositionalParameters, havePropertiesOnNewLine);
    }
    public static PropertyCreationMethod InitAutoProperties() => new(PropertyCreationType.InitAutoProperties, null);
}
