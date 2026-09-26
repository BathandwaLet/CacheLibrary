namespace CacheLibrary.Core;

public class TtlCache<TKey, TValue>: ICache<TKey, TValue>
{
    //Dictionary and linkedlist
    private TtlNode<TKey, TValue>? _head = null;
    private TtlNode<TKey, TValue>? _tail = null;
    private Dictionary<TKey, TtlNode<TKey, TValue>> TtlQueue = new();
    private readonly int _cacheCapacity;

    public TtlCache(int cacheCapacity)
    {
        _cacheCapacity = cacheCapacity;
    }
    public bool TryGetValue(TKey key, out TValue value)
    {
        throw new NotImplementedException();
    }

    public bool TrySetValue(TKey key, TValue value)
    {
        throw new NotImplementedException();
    }

    public bool TryRemoveValue(TKey key)
    {
        throw new NotImplementedException();
    }
}