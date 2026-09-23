namespace CacheLibrary.Core;

public class LfuCache <TKey , TValue> : ICache<TKey , TValue>
{
    private Dictionary<TKey, LfuNode<TKey, TValue>> LfuQueue {get; set; } = new();
    private Dictionary<int, FrequencyList<TKey, TValue>> FrequencyLists { get; set; } = new();
    private int _minFrequency;
    private readonly int _cacheCapacity;

    public LfuCache(int cacheCapacity)
    {
        _cacheCapacity = cacheCapacity;
        _minFrequency = 1;
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