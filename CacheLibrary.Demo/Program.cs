using System;
using System.Collections.Generic;
using CacheLibrary.Core;
using CacheLibrary.Demo;


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

        ConsoleActions consoleAction = ConsoleActions.none;
        
        while (true)
        {
            Console.WriteLine("Please enter:" +
                              "\n'set' to set a new node to cache" +
                              "\n'get' to get a node from cache" +
                              "\n'remove' to remove a node from cache" +
                              "\n'quit' to exit.");
            
            string consoleActionInput = Console.ReadLine()?.ToLower();

            consoleAction = ParseActionToEnum(consoleActionInput);

            bool selectedQuit = false;
            
            switch (consoleAction)
            {
                case ConsoleActions.set:
                    //set the node
                    Console.WriteLine("Enter a key value:");
                    string? key = Console.ReadLine();

                    int value;
                    while (true)
                    {
                        Console.WriteLine("Enter a value:");

                        if (!int.TryParse(Console.ReadLine(), out value))
                        {
                            Console.WriteLine("Invalid input.\nTry entering 1, 5 or 77.");
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    if (cache is TtlCache<string, int> ttlCache)
                    {
                        
                        TimeSpan duration;
                        while (true)
                        {
                            Console.WriteLine("Enter a duration in seconds:");

                            if (!int.TryParse(Console.ReadLine(), out int durationInput))
                            {
                                Console.WriteLine("Invalid input.\nTry entering 1, 30 or 600.");
                            }
                            else
                            {
                                duration = TimeSpan.FromSeconds(durationInput);
                                break;
                            }
                        }

                        if (ttlCache.TrySetValue(key, value, duration))
                        {
                            Console.WriteLine($"{key} was successfully added to the cache.");
                        }

                    }
                    else
                    {
                        
                        if (cache.TrySetValue(key, value))
                        {
                            Console.WriteLine($"{key} was successfully added to the cache.");
                        }
                    }
                    break;
                
                case ConsoleActions.get: 
                    //get the node
                    Console.WriteLine("Enter a key:");
                    string? getKey = Console.ReadLine();

                    if (cache.TryGetValue(getKey, out int getValue))
                    {
                        Console.WriteLine($"The key:{getKey} has a value of {getValue}.");
                    }
                    else
                    {
                        Console.WriteLine($"{getKey} was not found in the cache.");
                    }
                    break;
                
                case ConsoleActions.remove:
                    //remove the node
                    Console.WriteLine("Enter a key:");
                    string? removeKey = Console.ReadLine();
                    if (cache.TryRemoveValue(removeKey))
                    {
                        Console.WriteLine($"The key:{removeKey} has been removed from cache.");
                    }
                    else
                    {
                        Console.WriteLine($"{removeKey} was not found in the cache.");
                    }
                    break;
                
                case ConsoleActions.quit:
                    selectedQuit = true;
                    break;
                
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }

            if (selectedQuit)
            {
                break;
            }
        }
        
    }

    private static ConsoleActions ParseActionToEnum (string input)
    {
        return input switch
        {
            "set" => ConsoleActions.set,
            "get" => ConsoleActions.get,
            "remove" => ConsoleActions.remove,
            "quit" => ConsoleActions.quit,
            _ => ConsoleActions.none
        };
    }
}