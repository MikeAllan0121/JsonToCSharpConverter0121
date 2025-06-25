namespace JsonToCSharpConverter0121;

public class HavePropertiesOnNewLine
{
    private const int DEFAULT_NUMBER_OF_LINES = 5;

    public bool IsSet { get; }
    public bool IsNotSet => !IsSet;

    private int _afterNumberOfProperties = DEFAULT_NUMBER_OF_LINES;
    public int AfterNumberOfProperties
    { 
        get => IsSet ? _afterNumberOfProperties : throw new Exception($"Only accessable if object {nameof(IsSet)} property is true"); 
        set => _afterNumberOfProperties = value; 
    }

    public bool RequiresNewLine(int count) => IsSet && AfterNumberOfProperties <= count;

    public bool DoesNotRequiresNewLine(int count) => !RequiresNewLine(count);


    private HavePropertiesOnNewLine(bool isSet, int afterNumberOfProperties)
    {
        AfterNumberOfProperties = afterNumberOfProperties;
        IsSet = isSet;
    }

    private HavePropertiesOnNewLine(bool isSet)
    {
        IsSet = isSet;
    }

    public static HavePropertiesOnNewLine Yes(int numberOfLines = DEFAULT_NUMBER_OF_LINES) => new(true, numberOfLines); 
    public static HavePropertiesOnNewLine No() => new(false);

}
