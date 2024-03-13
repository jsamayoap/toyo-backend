namespace code.interfaces;

public interface IRelationalEntity<TKey, TConcurrency>
        where TConcurrency : struct
{
    TKey Key { get; set; }
    TConcurrency? Rv { get; set; }
}