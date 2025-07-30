using JsonToCSharpConverter0121.Internal.Records.Naming;

namespace JsonToCSharpConverter0121.Internal.Records;

internal interface IRecords
{
    Record this[NameBuilder name] { get; }

    IEnumerable<Record> Items { get; }
}
