using Extensions0121.Exceptions;
using System.Collections.Generic;

namespace JsonToCSharpConverter0121.Internal.Records.Naming;

internal record NameBuilder
{
    private readonly List<NameGiver> _names = [];

    private readonly TypeNamingConvention _typeNamingConvention;

    public string FullCsharpName => string.Join("", _names.Select(n => n.CSharpName));

    public string CsharpName => _names.Last().CSharpName;

    public string Name => _names.Last().Name;

    private NameBuilder(TypeNamingConvention typeNamingConvention)
    {
        _typeNamingConvention = typeNamingConvention;
    }

    public NameBuilder(string name, TypeNamingConvention typeNamingConvention)
    {
        _names.Add(new(name));

        _typeNamingConvention = typeNamingConvention;
    }

    public NameBuilder ExtendNameBuilder(string name) => _typeNamingConvention switch
        {
            TypeNamingConvention.PropertyName => new(name, _typeNamingConvention),
            TypeNamingConvention.NestedPositionName => NestedPositionName(name),
            _ => throw ThrowHelper.EnumExhausted(nameof(TypeNamingConvention)),
        };

    private NameBuilder NestedPositionName(string name)
    {
        NameBuilder copy = new(_typeNamingConvention);
        foreach (var n in _names)
        {
            copy._names.Add(n);
        }
        copy._names.Add(new(name));
        return copy;
    }

    public virtual bool Equals(NameBuilder? other)
    {
        if (ReferenceEquals(this, other))
            return true;
        if (other is null)
            return false;
        if (_typeNamingConvention != other._typeNamingConvention)
            return false;
        if (_names.Count != other._names.Count)
            return false;
        for (int i = 0; i < _names.Count; i++)
        {
            if (!_names[i].Equals(other._names[i]))
            {
                return false;
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var name in _names)
            hash.Add(name);
        hash.Add(_typeNamingConvention);
        return hash.ToHashCode();
    }
}
