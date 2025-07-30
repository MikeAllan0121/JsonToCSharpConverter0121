namespace JsonToCSharpConverter0121.Internal.Records.Naming;

public record NameGiver(string Name)
{
    public string CSharpName => Name.ConvertToCSharpPropertyName();
}