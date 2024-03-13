using code.interfaces;

namespace code.common;

public abstract class RelationalEntity<TKey, TConcurrency> : IRelationalEntity<TKey, TConcurrency>, IEquatable<RelationalEntity<TKey, TConcurrency>>
        where TKey : IEquatable<TKey>
        where TConcurrency : struct
{
    public abstract TKey Key { get; set; }
    public virtual TConcurrency? Rv { get; set; }

    public bool Equals(RelationalEntity<TKey, TConcurrency>? other) => other != null && Key.Equals(other.Key);

    public override bool Equals(object? obj) => Equals(obj as RelationalEntity<TKey, TConcurrency>);

    public override int GetHashCode() => base.GetHashCode();
}