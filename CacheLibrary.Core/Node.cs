namespace CacheLibrary.Core;

public class Node <TKey, TValue>
{
    public TKey Key;
    public TValue Value;
    public Node<TKey, TValue> Prev;
    public Node<TKey, TValue> Next;
}