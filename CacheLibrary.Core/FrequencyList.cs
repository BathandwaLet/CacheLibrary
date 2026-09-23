namespace CacheLibrary.Core;

//Doubly linked list to store the process for LFU cache implementation
public class FrequencyList <TKey, TValue>
{
    public LfuNode<TKey, TValue>? Head { get; set; }
    public LfuNode<TKey, TValue>? Tail { get; set; }
}