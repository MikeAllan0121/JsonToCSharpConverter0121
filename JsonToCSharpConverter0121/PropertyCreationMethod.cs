namespace JsonToCSharpConverter0121;

/// <summary>
/// Specifies how properties are generated in C# record types during JSON-to-C# conversion.
/// </summary>
public class PropertyCreationMethod
{
    /// <summary>
    /// The method used for property creation, such as positional parameters or init-only auto-properties.
    /// </summary>
    public PropertyCreationType Type { get; private set; }

    private readonly HavePropertiesOnNewLine? _havePropertiesOnNewLine;

    /// <summary>
    /// Controls whether properties should be placed on new lines when using positional parameters.
    /// Only valid when <see cref="Type"/> is <see cref="PropertyCreationType.PositionalParameters"/>.
    /// </summary>
    /// <exception cref="Exception">
    /// Thrown if accessed when <see cref="Type"/> is not <see cref="PropertyCreationType.PositionalParameters"/>,
    /// or if the value is not set for positional parameters.
    /// </exception>
    public HavePropertiesOnNewLine HavePropertiesOnNewLine =>
        Type == PropertyCreationType.PositionalParameters
        ? _havePropertiesOnNewLine ?? throw new Exception("HavePropertiesOnNewLine is not set for PositionalParameters")
        : throw new Exception("Type not valid for this property to be set");

    /// <summary>
    /// Initializes a new instance of <see cref="PropertyCreationMethod"/> with the specified type and new-line option.
    /// </summary>
    /// <param name="type">The property creation type.</param>
    /// <param name="havePropertiesOnNewLine">The new-line option for positional parameters.</param>
    private PropertyCreationMethod(PropertyCreationType type, HavePropertiesOnNewLine? havePropertiesOnNewLine)
    {
        Type = type;
        _havePropertiesOnNewLine = havePropertiesOnNewLine;
    }

    /// <summary>
    /// Creates a <see cref="PropertyCreationMethod"/> for positional parameters.
    /// </summary>
    /// <param name="havePropertiesOnNewLine">
    /// Optionally specifies if and when properties should be placed on new lines.
    /// Defaults to <see cref="HavePropertiesOnNewLine.Yes()"/> if not provided.
    /// </param>
    /// <returns>A <see cref="PropertyCreationMethod"/> configured for positional parameters.</returns>
    public static PropertyCreationMethod PositionalParameters(HavePropertiesOnNewLine? havePropertiesOnNewLine = null)
    {
        havePropertiesOnNewLine ??= HavePropertiesOnNewLine.Yes();
        return new PropertyCreationMethod(PropertyCreationType.PositionalParameters, havePropertiesOnNewLine);
    }

    /// <summary>
    /// Creates a <see cref="PropertyCreationMethod"/> for init-only auto-properties.
    /// </summary>
    /// <returns>A <see cref="PropertyCreationMethod"/> configured for init-only auto-properties.</returns>
    public static PropertyCreationMethod InitAutoProperties() => new(PropertyCreationType.InitAutoProperties, null);
}
