using System;
using System.Collections.Generic;

namespace Banan
{

    // Represents an item that can be stored in the inventory
    class Item
    {
        public string Name { get; } // Name of the item (e.g., "Banana", "Tutorial")
        public string Type { get; } // Type/category of the item (e.g., "food", "knowledge")

        public Item(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }

    // Represents the player's inventory, allowing adding, using, and displaying items
    class Inventory
    {
        // Stores items by name, with a tuple of the item and its count
        private Dictionary<string, (Item item, int count)> items = new();

        // Adds an item to the inventory (increments count if already present)
        public void Add(Item item)
        {
            if (items.ContainsKey(item.Name))
            {
                var (existingItem, count) = items[item.Name];
                items[item.Name] = (existingItem, count + 1);
            }
            else
            {
                items[item.Name] = (item, 1);
            }
        }

        // Uses (removes) one instance of an item by name
        public void Use(string name)
        {
            if (!items.ContainsKey(name)) return;

            var (item, count) = items[name];

            if (count > 1)
                items[name] = (item, count - 1);
            else
                items.Remove(name);
        }

        // Displays the inventory contents in the console at a given Y position
        public void Show(int startY, int maxLines = 10)
        {
            ClearArea(startY, maxLines);

            Console.SetCursorPosition(0, startY);
            Console.WriteLine("Inventory:");

            int line = 1;
            foreach (var kv in items)
            {
                if (line >= maxLines) break;

                var (item, count) = kv.Value;
                Console.SetCursorPosition(0, startY + line);
                Console.WriteLine($"- {item.Name} ({item.Type}) x{count}");
                line++;
            }
        }

        // Clears the inventory display area in the console
        private void ClearArea(int startY, int maxLines)
        {
            for (int i = 0; i < maxLines + 2; i++)
            {
                Console.SetCursorPosition(0, startY + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        // Opens the inventory UI and waits for the user to press 'E' to exit
        public void OpenInventory(int startY, int maxLines = 10)
        {
            Show(startY, maxLines);

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.E)
                {
                    ClearArea(startY, maxLines);
                    break;
                }
            }
        }

        // Checks if the inventory contains an item by name
        public bool HasItem(string name)
        {
            return items.ContainsKey(name);
        }

        // Returns the count of a specific item by name
        public int Count(string name)
        {
            return items.ContainsKey(name) ? items[name].count : 0;
        }
    }
}
