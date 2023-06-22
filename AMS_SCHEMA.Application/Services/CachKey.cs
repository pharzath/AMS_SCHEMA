namespace AMS.Model.Services;

public class CachKey : IEquatable<CachKey>
{
    public required Type Type { get; init; }
    public long? Id { get; init; }
    public string? Name { get; set; }
    public Type[]? RelatedTypes { get; init; }
    public CachKey[]? RelatedKeys { get; init; }

    public CachKey()
    {

    }
    public CachKey(string? name)
    {
        Name = name;
    }


    public bool Equals(CachKey? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Type == other.Type &&
               Id == other.Id &&
               Name == other.Name &&
               (RelatedTypes != null && other.RelatedTypes != null && RelatedTypes.Equals(other.RelatedTypes)) &&
               (RelatedKeys != null && other.RelatedKeys != null && RelatedKeys.Equals(other.RelatedKeys));
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CachKey)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Id, Name, RelatedTypes, RelatedKeys);
    }
}