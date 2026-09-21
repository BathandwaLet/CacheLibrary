using Xunit;
using CacheLibrary.Core;

namespace CacheLibrary.Tests;

public class LruCacheTests
{
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
}