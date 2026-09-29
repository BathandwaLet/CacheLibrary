using System;
using System.Collections.Generic;
using CacheLibrary.Core;


class Program
{
    static void Main(string[] args)
    {
        ICache<string, int> cache;
        
        Console.WriteLine("Welcome to the Cache Library Simulator");

        int memoryManagSelection = 0;
        int cacheCapacity = 0;
        
        while (true)
        {
            Console.WriteLine("Please select the memory management strategy (1-3)" + 
                              "\n1. Least Recently Used (LRU)" + 
                              "\n2. Least Frequently Used (LFU)" + 
                              "\n3. Hybrid Time To Live (TTL)");

            if (!int.TryParse(Console.ReadLine(), out memoryManagSelection) || 
                memoryManagSelection < 1 || memoryManagSelection > 3)
            {
                Console.WriteLine("Invalid selection, Please enter either 1, 2 or 3.");
                continue;
            }
            
            break;
        }
        
        while (true)
        {
            Console.WriteLine("Please enter the cache capacity");

            if (!int.TryParse(Console.ReadLine(), out  cacheCapacity) || cacheCapacity < 1)
            {
                Console.WriteLine("Invalid selection, Please enter a whole number greater than or equal to 1.");
                continue;
            }

            break;
        }

        switch (memoryManagSelection)
        {
            case 1:
                cache = new LruCache<string, int>(cacheCapacity);
                break;
            case 2:
                cache = new LfuCache<string, int>(cacheCapacity);
                break;
            case 3:
                cache = new TtlCache<string, int>(cacheCapacity);
                break;
            default:
                throw new InvalidOperationException(); 
                break;
        }
        
    }
}