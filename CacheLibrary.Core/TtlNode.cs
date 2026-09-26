namespace CacheLibrary.Core;

public class TtlNode<TKey, TValue> : Node<TKey, TValue>
{
    public DateTime ExpiresAt { get; set; }
    public TimeSpan LifeTime { get; set; }
    public new TtlNode<TKey, TValue>? Prev { get; set; }
    public new TtlNode<TKey, TValue>? Next { get; set; }

    public TtlNode(TKey key, TValue value, TimeSpan lifeTime) : base(key, value)
    {
        LifeTime = lifeTime;
        ExpiresAt = DateTime.Now + LifeTime;
    }
}