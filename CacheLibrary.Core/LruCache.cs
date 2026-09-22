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
        bool hasKey = LruQueue.TryGetValue(key, out Node <TKey, TValue> node);
        value = hasKey ? node.Value : default;
        
        
        //moving process to the front of the linkedlist
        if (!hasKey || node == _head)
        {
            return hasKey;
        }
        else if (node.Prev != null)
        {
            /*
             Move to helper methods
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
            */
            MoveToHead(node);
        }
        
        return hasKey ;
    }

    public bool TrySetValue(TKey key, TValue value)
    {
        bool hasKey = LruQueue.TryGetValue(key, out Node <TKey, TValue> node);
        //Case 1 Existing key => Update value then move to the head
        if (hasKey)
        {
            node.Value = value;
            MoveToHead(node);
            return true;
        }
        else
        {
            //Create a node to be referenced later
            var new_node = new Node<TKey, TValue>(key, value);
            
            //Case 2 New key and under cache capacity => Move to head
            int currentCapacity = LruQueue.Count;
            
            if (currentCapacity < _cacheCapacity)
            {
                InsertAtHead(new_node);
                LruQueue.Add(key, new_node);
            }
            else
            {
                //Case 3 New key and cache capacity => Evict and move to head
                //Evict
                if (_head == _tail)
                {
                    LruQueue.Remove(_tail.Key);
                    _head = null; 
                    _tail = null;
                }
                else
                {
                    LruQueue.Remove(_tail.Key);
                    _tail.Prev.Next = null;
                    _tail = _tail.Prev;
                }
                
                //To move to head
                InsertAtHead(new_node);
                LruQueue.Add(key, new_node);
            }

            return true;

        }
        return false;
    }

    public bool TryRemoveValue(TKey key)
    {
        throw new NotImplementedException();
    }

    //HELPER METHODS
    private void MoveToHead(Node<TKey, TValue> node)
    {
        //If the node is the head
        if (node == _head)
        {
            return;
        }
        
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
        _head = node;
    }

    private void InsertAtHead(Node<TKey, TValue> node)
    {
        if (_head == null)
        {
            _head = node;
            _tail = node;
        }
        else
        {
            //Link the new head
            _head.Prev = node;
            node.Next = _head;
            node.Prev = null;
            _head = node;
        }
    }
}