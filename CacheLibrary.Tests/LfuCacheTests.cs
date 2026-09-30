using Xunit;
using CacheLibrary.Core;

namespace CacheLibrary.Tests;

public class LfuCacheTests
{
    
       //TryGetValue
       
       // Miss on empty cache → false
       [Fact]
       public void TryGetValue_GetValueThatDoesNotExist_ReturnsFalse()
       {
              //arrange
              var cache = new LfuCache<string, int>(2);
              
              //act
              bool isFound = cache.TryGetValue("testkey", out int value);

              //assert
              Assert.False(isFound);
       }
       // Hit on existing key → true, correct value
       
       // Hit increments count and moves node to a new bucket (verify indirectly — e.g. access key A twice, key B once, force eviction, confirm B was evicted not A)
       // Hit on a key that's the only item in its bucket → bucket empties and gets removed correctly, no crash on subsequent operations
       
       //TrySetValue
       
       // New key, cache under capacity → retrievable afterward, starts at frequency 1
       // Existing key → value updates, count increments
       // New key, cache at capacity → evicts the correct LFU node (lowest count)
       // Tie-break: two keys with equal count, cache at capacity, insert new key → the least-recently-used of the tied pair gets evicted (proves your LRU-within-bucket tie-break works)
       // _minFrequency correctly resets to 1 after inserting a new key into a cache where all existing items have higher counts
       
       //TryRemoveValue
       
       // Remove missing key → false
       // Remove existing key that's the sole item in its bucket → true, _minFrequency recalculates correctly if it was the min bucket
       // Remove existing key that's not alone in its bucket → true, bucket survives with remaining items intact
     
}