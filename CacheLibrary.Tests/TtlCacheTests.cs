using Xunit;
using CacheLibrary.Core;

namespace CacheLibrary.Tests;

public class TtlCacheTests
{
    
        //TryGetValue  
       //1. Miss on an empty cache returns false.
       [Fact]
       public void TryGetValue_MissOnEmptyCache_ReturnsFalse()
       {
           //Arrange
           var cache = new TtlCache<string, int>(3);
           
           
           //Act
           var found = cache.TryGetValue("testkey", out var value);
           
           //Assert
           Assert.False(found);
       }
       
       //2. Key set with a long duration returns true and the correct value straight away.
       [Fact]
       public void TryGetValue_KeySetWithALongDuration_ReturnsTrue()
       {
           //Arrange
           var cache = new TtlCache<string, int>(2);
           cache.TrySetValue("testkey", 6, TimeSpan.FromMilliseconds(5000000));
           
           //Act
           var found = cache.TryGetValue("testkey", out var value);

           //Assert
           Assert.True(found); 
           Assert.Equal(6, value);
       }
       
       //3. Key set with a short duration returns false after the duration passes, and the out value is default.
       [Fact]
       public void TryGetValue_KeyExpired_ReturnsFalse()
       {
           // Arrange
           var cache = new TtlCache<string, int>(3);
           cache.TrySetValue("testkey", 5, TimeSpan.FromMilliseconds(50));

           // Act
           Thread.Sleep(100);
           var found = cache.TryGetValue("testkey", out var value);

           // Assert
           Assert.False(found);
           Assert.Equal(0,value);
       }
       
       //4. A live hit moves the key to the head. Capacity 2: set a and b, get a, set c,
       //then b should be evicted and a should survive.
       [Fact]
       public void TryGetValue_LiveHit_MovesKeyToHeadAndCacheEviction_ReturnsFalse()
       {
           //Arrange
           var cache = new TtlCache<string,int>(2);
           cache.TrySetValue("testkey", 6, TimeSpan.FromMilliseconds(50));
           cache.TrySetValue("testkey1", 7,  TimeSpan.FromMilliseconds(55));
           
           //Act
           var found = cache.TryGetValue("testkey", out var value);
           cache.TrySetValue("testkey2", 8,  TimeSpan.FromMilliseconds(60));

           var found1 = cache.TryGetValue("testkey2", out var value1);
           var found2 = cache.TryGetValue("testkey1", out var value2);
           
           //Assert
           Assert.True(found);
           Assert.True(found1);
           Assert.False(found2);
           
       }
       
       //5. Getting an expired key removes it from the cache. After the expired get, setting a new key into a full cache should not evict a live neighbour, 
       //because the expired one already freed its slot.
       [Fact]
       public void TryGetValue_GetExpiredKey_RemoveFromCache()
       {
           //Arrange
           var cache = new TtlCache<string, int>(2);
           cache.TrySetValue("testkey", 6, TimeSpan.FromMilliseconds(50));
           cache.TrySetValue("testkey1", 7, TimeSpan.FromMilliseconds(150));
           
           //Act
           Thread.Sleep(70);
           var foundTestKey = cache.TryGetValue("testkey", out var testKeyValue);
           var foundTestKey1 = cache.TryGetValue("testkey1", out var testKeyValue1);
           cache.TrySetValue("newtestkey", 8, TimeSpan.FromMilliseconds(100));
           var foundNewKey = cache.TryGetValue("newtestkey", out var newKeyValue);
           

           //Assert
            Assert.False(foundTestKey);
            Assert.True(foundTestKey1);
            Assert.True(foundNewKey);
       }
       
       //TrySetValue
       //6. New key under capacity is retrievable afterwards.
       
       //7. Existing live key updates the value and refreshes the expiry. Set a with a short duration, wait most of it, set a again with a longer duration, wait past the original expiry, and a should still be there.
       
       //8. Existing expired key is treated as a new key. Set a, let it expire, set a again, get a and get the new value. This is the ghost node case, so also check that the count didn't drift by filling the cache to capacity afterwards.
       
       //9. New key at capacity with nothing expired evicts the LRU tail (the hybrid fallback).
       
       //10. The overload without a duration uses the default. Set a key with no duration and confirm it's still readable immediately.
       
       //TryRemoveValue
       //11. Removing a missing key returns false.
       [Fact]
       public void TryRemoveValue_RemovesAMissingKey_ReturnsFalse()
       {
           //Arrange
           var cache = new TtlCache<string,int>(2);
           
           //Act
           var found = cache.TryRemoveValue("testkey");
           
           //Assert
           Assert.False(found);

       }
       
       //12. RRemoving a missing key returns false.
       [Fact]
       public void TryRemoveValue_RemovesALiveKey_ReturnsTrue_GetReturnFalse()
       {
           //arrange
           var cache = new TtlCache<string, int>(2);
           cache.TrySetValue("testkey", 1);
           cache.TrySetValue("testkey2", 2);
           
           //act
           var remove = cache.TryRemoveValue("testkey");
           var getlater = cache.TryGetValue("testkey", out var value);
           
           //assert
           Assert.True(remove);
           Assert.False(getlater);
       }
       //13. Removing an expired key returns false (matching TryGetValue) but still cleans it up. Get afterwards also returns false.
       //14. Removing the head, the tail, the middle and the only node each leave the list usable. Insert and get afterwards to prove it.
    
}