using Xunit;
using CacheLibrary.Core;

namespace CacheLibrary.Tests;

public class LruCacheTests
{
    //TryGetValue Tests
    [Fact]
    public void TryGetValue_Does_Not_Exist_Returns_False()
    {
        //Arrange
        var cache = new LruCache<string, int>(3);
        
        //Act
        var found = cache.TryGetValue("apple", out var value);
        
        //Assert
        Assert.False(found);
        Assert.Equal(0, value);
    }
    /*
       TODO
       Remaining tests for TryGetValue
       Hit on existing key → true, correct value
       Hit moves node to head (reorder correctness)
       Hit on a key that's already head → no crash, still returns correct value
     */
    
    //TrySetValue Tests
    [Fact]
    public void TrySetValue_NewKeyUnderCapacity_BecomesHead()
    {
        //Arrange
        var cache = new LruCache<string, int>(3);
        
        //Act
        var setResult = cache.TrySetValue("banana", 1);
        cache.TryGetValue("banana", out var value);
        
        //Assert
        Assert.True(setResult);
        Assert.Equal(1, value);
    }
    
    
    //TODO
    //Remaining TrySetValue test
    //Existing key → value updates, moves to head
    
    //New key, cache at capacity → evicts LRU (tail), new key becomes head
    
    //Single-item cache (capacity 1), insert second key → evicts the only item cleanly (your head==tail edge case)
    [Fact]
    public void TrySetValue_SingleItemCache_NewInsertion_EvictsThenAddNewKey_ReturnsTrue()
    {
        //arrange
        var cache = new LruCache<string, int>(1);
        cache.TrySetValue("testkey1", 1);

        //act
        bool setNewKey = cache.TrySetValue("testkey2", 2);
        bool isRemovedT1 = cache.TryGetValue("testkey1", out int value1);
        bool isRemovedT2 = cache.TryGetValue("testkey2", out int value2);
        
        //assert
        Assert.True(setNewKey);
        Assert.False(isRemovedT1);
        Assert.True(isRemovedT2);

    }
    
    //TryRemoveValue Tests
    
    //TODO
    //Remaining TryRemoveValue tests
    //Remove missing key → false
    //Remove existing key (middle of list) → true, list stays valid
    //Remove head → _head updates correctly
    [Fact]
    public void TryRemoveValue_RemovingTheHeadValue_ReturnsTrue()
    {
        //arrange
        var cache = new LruCache<string, int>(3);
        cache.TrySetValue("testkey1", 1);
        cache.TrySetValue("testkey2", 2);
        cache.TrySetValue("testkey3", 3);
        
        //act
        int value1, value2, value3 = 0;
        cache.TryGetValue("testkey1", out value1);
        cache.TryGetValue("testkey2", out value2);
        cache.TryGetValue("testkey2", out value2);
        cache.TryGetValue("testkey3", out value3);
        cache.TryGetValue("testkey3", out value3);
        cache.TryGetValue("testkey3", out value3);
        
        bool isRemoved = cache.TryRemoveValue("testkey3");
        bool stillInCache1 = cache.TryGetValue("testkey1", out value1);
        bool stillInCache2 = cache.TryGetValue("testkey2", out value2);
        bool stillInCache3 = cache.TryGetValue("testkey3", out value3);
        
        //assert
        Assert.True(isRemoved);
        Assert.True(stillInCache1);
        Assert.True(stillInCache2);
        Assert.False(stillInCache3);
    }
    
    //Remove tail → _tail updates correctly
    [Fact]
    public void TryRemoveValue_RemovingTheTailValue_ReturnsTrue()
    {
        //arrange
        var cache = new LruCache<string, int>(3);
        cache.TrySetValue("testkey1", 1);
        cache.TrySetValue("testkey2", 2);
        cache.TrySetValue("testkey3", 3);
        
        //act
        int value1, value2, value3 = 0;
        cache.TryGetValue("testkey1", out value1);
        cache.TryGetValue("testkey2", out value2);
        cache.TryGetValue("testkey2", out value2);
        cache.TryGetValue("testkey3", out value3);
        cache.TryGetValue("testkey3", out value3);
        cache.TryGetValue("testkey3", out value3);

        bool isRemoved = cache.TryRemoveValue("testkey1");
        bool stillInCache1 = cache.TryGetValue("testkey1", out value1);
        bool stillInCache2 = cache.TryGetValue("testkey2", out value2);
        bool stillInCache3 = cache.TryGetValue("testkey3", out value3);

        //assert
        Assert.True(isRemoved);
        Assert.False(stillInCache1);
        Assert.True(stillInCache2);
        Assert.True(stillInCache3);

    }
    
    //Remove the only item in a single-item cache → _head/_tail both become null
    [Fact]
    public void TryRemoveValue_RemovingTheOnlyItemInCache_HeadandTailBecomeNull_ReturnsTrue()
    {
        //arrange
        var cache = new LruCache<string, int>(3);
        
        //act
        cache.TrySetValue("testkey", 1);
        cache.TryRemoveValue("testkey");
        bool isInCache = cache.TryGetValue("testkey", out var value);

        //assert
        Assert.Equal(0, value);
        Assert.False(isInCache);
    }
}