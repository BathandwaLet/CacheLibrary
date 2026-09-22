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
    
    //TryRemoveValue Tests
}