# CACHE LIBRARY SIMULATOR
## OVERVIEW
**Cache Library**
This is a console-based application that aims to allow users to explore and compare three main cache memory management algorithms.

This project scope covers:
- Least Recently Used (LRU) memory management algorithm.
- Least Frequently Used (LFU) memory management algorithm.
- Hybrid Time-to-Live (TTL) memory management algorithm.
- Unit tests for each memory management strategy.
- Console-based demo to show the memory management algorithms at work.

## FEATURES
- Console-based UI interface, letting the user pick a strategy and run set, get and remove commands interactively.
- Three interchangeable cache implementations (LRU, LFU, TTL) built against a shared ICache<TKey, TValue> interface, so any of the three can be swapped in without changing calling code.
- Generic key/value types throughout the library (CacheLibrary.Core), not tied to any specific data type.
- A TTL-specific overload allowing a custom expiry duration per key, with a sensible default when none is given.
- Self-implemented doubly linked list and node structures for all three strategies, rather than relying on .NET's built-in LinkedList, to demonstrate the underlying data structure mechanics directly.

## SCREENSHOTS
<img width="1440" height="900" alt="Screenshot 2026-09-30 at 05 16 33" src="https://github.com/user-attachments/assets/aa23188a-6c2b-4749-80e5-d08b03c05621" />
<img width="1440" height="900" alt="Screenshot 2026-09-30 at 05 16 37" src="https://github.com/user-attachments/assets/164b815b-8d93-4b73-b21b-5375512d43fa" />
Screenshots of the demo running using the LFU algorithm, setting and removing nodes

## TECH STACK
- Programming Language: **C#**
- Framework: **.NET 10**
- Testing: **XUnit**
- IDE: **Rider**
- Version Control: **Git**

## HOW TO RUN
- Clone the repository.  
- Open in any IDE that supports C# with .NET 10.0 (Visual Studio, Rider, etc).
- Build the solution.
- Run the CacheLibrary.Demo.

## DESIGN DECISIONS
- LRU is built using a dictionary and a doubly linked list. This allows us to have an O(1) get and put. The dictionary maps each key to its node; the list is used to track the LRU queue. The head would be the most recently used, and the tail would be the least recently used. Each node stores its own key, so when the node removed from the tail it can be removed in O(1) instead of traversing through the list which is. O(n).
- LFU extends on this concept by also using a dictionary with a doubly-linked list. The dictionary maps each frequency count to a 'bucket' (This is another small doubly linked list of the nodes with the same count. The minFrequency value always points to the minimum frequency, therefore skipping the need to scan for it. Within the bucket, the nodes are arranged in the same order as LRU (the head is the most recent and the tail is the least recent), and in the case of eviction, the tail is always evicted first.
- TTL uses the lazy expiration approach. Expiry is only checked when the key is actually accessed (TryGetValue) or set (TrySetValue). This is has the cost of the expired node sitting in the cache longer than its exact lifetime.
- TTL defaults to LRU as a tie-breaking mechanism if nothing has expired yet. The proper implementation of TTL has no preferred ordering rule for this situation. LRU was chosen to avoid introducing another data structure to queue incoming nodes until another node expires and is evicted.
- LfuNode and TtlNode inherit from the shared Node class. LfuNode adds a Count field to track the frequency; TtlNode adds ExpiresAt and LifeTime. Both hide the inherited Prev/Next properties so they can resolve with their own node type without the need for casts throughout the cache classes.
- TryRemoveValue on LFU cache is a deliberate exception to the O(1) performance requirement. When the removed key is the only node in the minimum-frequency bucket, there is no truly efficient way to find the next/new minimum other than looping through the dictionary.
- All of the caches silently reset a cacheCapacity below 1 to 1, so the library will never end up in an invalid state. The console demo enforces its own input validation (rejecting anything less than 1) so a user is told about the improper entry.

## ALGORITHMIC ANALYSIS (BIG O) SUMMARY
|STRATEGY|GET|SET|REMOVE|NOTES|
|-----|-----|-----|-----|-----|
|LRU|O(1)|O(1)|O(1)|This was achieved by using a combination of the linked list and dictionary.|
|LFU|O(1)|O(1)|O(n)|The Remove function degrades from O(1) to O(n) in the specific case that it was the sole node in the minimum frequency bucket and the algorithm has to search and set for the new minimum frequency.|
|TTL|O(1)|O(1)|O(1)|Lazy approach adds a constant time expiry check and eviction defaults to LRU.|

## TESTING
- Framework(s): xUnit located in the CacheLibrary.Tests project. It utilises the arrange, act, assert pattern.
- Coverage table
  |CACHE|TryGetValue|TrySetValue|TryRemoveValue|
  |-----|-----|-----|-----|
  |LRU||||
  |LFU||||
  |TTL|Miss on an empty cache, Hit on cache, Miss on an expired node, Cache hit that updates to node position to the head, Insert a new node in the cache after evicting an expired node|Set a new key for a cache under capacity, Update an existing key and updates the duration, Updating an expired key gets treated as a new key and ensure not ghost nodes are left behind, ||
- Trickiest cases to test
  1) The expired-node replacement in the TTL (TrySetValue_UpdateExistingExpiredKey_Without_LeavingAGhostNode_ReturnsTrue) in particular checking that no ghost node is left in cache. I had to hand trace and figure out what assertions held true or false in this case.
- Known Gaps
  1) No test for setting invalid values (anything less than 1) for the cacheCapacity to ensure the program sets it to a default value of 1.  


## KNOWN LIMITATIONS
- TTL algorithm uses the lazy expiration approach. This means an expired item can remain in memory until it is accessed or evicted.
- LFU's TryRemoveValue has a worse case of. O(n). Please see the table for a more detailed explanation.
- Cache capacity is silently clamped to 1 for negative and zero inputs, instead of rejecting. 
- No thread safety for the three caches. All data structures assume single-threaded access. Concurrent use was not in within the initial scope of this project.

## LICENSE
This project is licensed under the MIT License.  
See the LICENSE file for further information. 

## AUTHOR
**Bathandwa L Maphumulo**  
Email: bmap750@gmail.com  
LinkedIn: [in/bathandwa-maphumulo-216177180](https://www.linkedin.com/in/bathandwa-maphumulo-216177180/)
