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
        //Checking and then fetching the value
        var hasKey = LruQueue.TryGetValue(key, out Node <TKey, TValue> node);
        value = hasKey ? node.Value : default;
        
        //moving process to the front of the linkedlist
        if (!hasKey || node == _head)
        {
            return hasKey;
        }
        else if (node.Prev != null)
        {
            //Unlink the node
            // If the Node is tail
            if (node.Next == null) 
            {
                node.Prev.Next = null;
                _tail = node.Prev;
            }
            else
            {
                //if in between two nodes
                node.Prev.Next = node.Next;
                node.Next.Prev = node.Prev;
            }
            
            //Link the new head
            _head.Prev = node;
            node.Next = _head;
            node.Prev = null;
            
            //set the new head
            _head = node;

        }
        
        return hasKey ;
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