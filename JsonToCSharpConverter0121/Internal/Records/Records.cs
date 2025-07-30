using JsonToCSharpConverter0121.Internal.Records.Naming;

namespace JsonToCSharpConverter0121.Internal.Records;

internal class Records : IRecords
{
    private readonly List<Record> _items = [];
    public IEnumerable<Record> Items => _items;
    public bool ContainsKey(NameBuilder name) => _items.Any(r => r.Name.FullCsharpName == name.FullCsharpName);

    public void AddIfNew(NameBuilder name)
    {
        if (!ContainsKey(name))
        {
            _items.Add(new Record(name));
        }
    }
    public Record this[NameBuilder name]
    {
        get => _items.FirstOrDefault(r => r.Name.FullCsharpName == name.FullCsharpName) ?? throw new KeyNotFoundException($"Record with name '{name}' not found.");
    }
}
