namespace CacheLibrary.Core;

public class TtlCache<TKey, TValue>: ICache<TKey, TValue>
{
    
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