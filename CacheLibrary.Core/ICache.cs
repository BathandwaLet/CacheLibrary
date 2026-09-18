namespace CacheLibrary.Core;

public interface ICache<TKey, TValue>
{
    public bool TryGetValue(TKey key, out TValue value);
    public bool TrySetValue(TKey key, TValue value);
    public bool TryRemoveValue(TKey key);
}