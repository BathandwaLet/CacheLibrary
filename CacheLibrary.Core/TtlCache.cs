namespace CacheLibrary.Core;

public class TtlCache<TKey, TValue>: ICache<TKey, TValue>
{
    private TtlNode<TKey, TValue>? _head = null;
    private TtlNode<TKey, TValue>? _tail = null;
    private Dictionary<TKey, TtlNode<TKey, TValue>> TtlQueue = new();
    private readonly int _cacheCapacity;
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromMinutes(5);

    public TtlCache(int cacheCapacity)
    {
        //safeguard and reject a cacheCapacity less than 1
        _cacheCapacity = cacheCapacity < 1 ? 1 : cacheCapacity;
    }
    public bool TryGetValue(TKey key, out TValue value)
    {
        bool hasKey = TtlQueue.TryGetValue(key, out TtlNode <TKey, TValue> ttlNode);
        value = hasKey ? ttlNode.Value : default;

        if (hasKey)
        {
            if (!IsExpired(ttlNode))
            {
                MoveToHead(ttlNode);
            }
            else
            {
                TtlQueue.Remove(ttlNode.Key);
                RemoveFromLinkedList(ttlNode);
                hasKey = false;
                value = default;
            }
        }

        return hasKey;
    }

    public bool TrySetValue(TKey key, TValue value)
    {
        return TrySetValue(key, value, DefaultDuration);
    }
    
    //overload the method
    public bool TrySetValue(TKey key, TValue value, TimeSpan duration)
    {
        bool hasKey = TtlQueue.TryGetValue(key, out TtlNode <TKey, TValue> ttlNode);
        bool isExpired = hasKey != false && IsExpired(ttlNode);
        
        //Removes the expired node from the dictionary and the linkedlist.
        if (hasKey && isExpired)
        {
            TtlQueue.Remove(ttlNode.Key);
            RemoveFromLinkedList(ttlNode);
        }
        
        if (hasKey && !isExpired)
        {
            //value refreshes the ExpiresAt
            ttlNode.Value = value;
            ttlNode.LifeTime = duration;
            ttlNode.ExpiresAt = DateTime.Now + duration;
            MoveToHead(ttlNode);
        }
        else
        {
            int currentCapacity = TtlQueue.Count;
            var newTtlNode = new TtlNode<TKey, TValue>(key, value, duration);
            
            if (currentCapacity < _cacheCapacity)
            {
                //New key under capacity
                InsertAtHead(newTtlNode);
                TtlQueue.Add(key, newTtlNode);
            }
            else
            {
                //Using LRU algorithm
                //Evict the LRU node
                //Add the newnode
                TtlQueue.Remove(_tail.Key);
                RemoveFromLinkedList(_tail);
                InsertAtHead(newTtlNode);
                TtlQueue.Add(key, newTtlNode);
            }
        }
        return true;
    }

    public bool TryRemoveValue(TKey key)
    {
        
    }
    
    //Helper Methods
    public bool IsExpired(TtlNode<TKey, TValue> ttlNode)
    {
        return DateTime.Now > ttlNode.ExpiresAt;
    }
    
    //Adapted from LruCache
    private void MoveToHead(TtlNode<TKey, TValue> ttlNode)
    {
        //If the node is the head
        if (ttlNode == _head)
        {
            return;
        }
        
        //Unlink the node
        // If the Node is tail
        if (ttlNode.Next == null)
        {
            ttlNode.Prev.Next = null;
            _tail = ttlNode.Prev;
        }
        else
        {
            //if in between two nodes
            ttlNode.Prev.Next = ttlNode.Next;
            ttlNode.Next.Prev = ttlNode.Prev;
        }
        
        //Link the new head
        _head.Prev = ttlNode;
        ttlNode.Next = _head;
        ttlNode.Prev = null;
        _head = ttlNode;
    }

    private void RemoveFromLinkedList(TtlNode<TKey, TValue> ttlNode)
    {
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        else if (ttlNode == _head)
        {
            ttlNode.Next.Prev = null;
            _head = ttlNode.Next;
            ttlNode.Next = null;
        }
        else if (ttlNode == _tail)
        {
            ttlNode.Prev.Next = null;
            _tail = ttlNode.Prev;
            ttlNode.Prev = null;
        }
        else
        {
            ttlNode.Prev.Next = ttlNode.Next;
            ttlNode.Next.Prev = ttlNode.Prev;
            ttlNode.Prev = null;
            ttlNode.Next = null;
        }
    }
    
    //Adapted form LRUCache
    private void InsertAtHead(TtlNode<TKey, TValue> ttlNode)
    {
        if (_head == null)
        {
            _head = ttlNode;
            _tail = ttlNode;
        }
        else
        {
            //Link the new head
            _head.Prev = ttlNode;
            ttlNode.Next = _head;
            ttlNode.Prev = null;
            _head = ttlNode;
        }
    }
}