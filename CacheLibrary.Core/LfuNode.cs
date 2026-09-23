namespace CacheLibrary.Core;

public class LfuNode <TKey, TValue> : Node<TKey, TValue>
{
    public int Count { get; set; }
    
    public LfuNode(TKey key, TValue value):base(key, value)
    {
        Count = 1;
    }
}