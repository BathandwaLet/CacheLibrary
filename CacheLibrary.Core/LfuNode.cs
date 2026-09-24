namespace CacheLibrary.Core;

public class LfuNode <TKey, TValue> : Node<TKey, TValue>
{
    public int Count { get; set; }
    public new LfuNode<TKey, TValue>? Prev { get; set; }
    public new LfuNode<TKey, TValue>? Next { get; set; }
    
    public LfuNode(TKey key, TValue value):base(key, value)
    {
        Count = 1;
    }
}