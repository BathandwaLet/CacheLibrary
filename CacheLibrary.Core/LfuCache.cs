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
        // does key exist and fetch the value if not default value
        bool hasKey = LfuQueue.TryGetValue(key, out LfuNode <TKey, TValue> lfuNode);
        value = hasKey ? lfuNode.Value : default;

        // if not return false
        if (!hasKey)
        {
            return hasKey;
        }
        else
        {
            // if yes then
            //1 remove from the previous list into n + 1
            FrequencyLists.Remove(key);
            //2 increase the node count, 3 add to the n + 1
            if (_minFrequency ==
                lfuNode.Count && /*check the number of items frequency list possibly add var to track this*/)
            {
                _minFrequency++;
            }
            FrequencyLists.Add(lfuNode.Count++, lfuNode);
            

        }
        return hasKey;
    }

    public bool TrySetValue(TKey key, TValue value)
    {
        throw new NotImplementedException();
    }

    public bool TryRemoveValue(TKey key)
    {
        throw new NotImplementedException();
    }
    
    //HELPER METHODS
    public bool RemoveFromFreqBucket(LfuNode<TKey, TValue> lfuNode)
    {
        //unlink the node wherever it is in the list
        var freqBucket =  FrequencyLists[lfuNode.Count];
        
        //Only this node in the bucket
        if (freqBucket.Head == freqBucket.Tail)
        {
            //list is empty then remove entry from the bucket and update the minfreq
            freqBucket.Head = null;
            freqBucket.Tail = null;
            int intKey = lfuNode.Count;
            FrequencyLists.Remove(intKey);

            if (lfuNode.Count == _minFrequency)
            {
                return true;
            }
           
        }
        else if (lfuNode == freqBucket.Head)
        {
            lfuNode.Next.Prev = null;
            freqBucket.Head = lfuNode.Next;
            lfuNode.Next = null;
        }
        else if (lfuNode == freqBucket.Tail)
        {
            lfuNode.Prev.Next = null;
            freqBucket.Tail = lfuNode.Prev;
            lfuNode.Prev = null;
        }
        else
        {
            lfuNode.Prev.Next = lfuNode.Next;
            lfuNode.Next.Prev = lfuNode.Prev;
            lfuNode.Prev = null;
            lfuNode.Next = null;
        }

        return false;
    }

    public void InsertIntoBucket(int frequency, LfuNode<TKey, TValue> lfuNode)
    {
        //does the freqBucket exist yet?
        if (!FrequencyLists.ContainsKey(frequency))
        {
            FrequencyLists.Add(frequency, new FrequencyList<TKey, TValue>());
        }
        
        var freqBucket = FrequencyLists[frequency];

        if (freqBucket.Head == null)
        {
            freqBucket.Head = lfuNode;
            freqBucket.Tail = lfuNode;
        }
        else
        {
            freqBucket.Head.Prev = lfuNode;
            lfuNode.Next = freqBucket.Head;
            lfuNode.Prev = null;
            freqBucket.Head = lfuNode;
        }
    }
}