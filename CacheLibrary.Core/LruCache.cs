namespace CacheLibrary.Core;

public class LruCache<TKey, TValue>: ICache<TKey, TValue>
{
    private Node<TKey, TValue>? _head;
    private Node<TKey, TValue>? _tail;
    private Dictionary<TKey, Node<TKey, TValue>> LruQueue { get; set; } = new();
    private readonly int _cacheCapacity;

    public LruCache(int cacheCapacity)
    {
        _cacheCapacity = cacheCapacity;
    }

    //TODO get set remove to be defined
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