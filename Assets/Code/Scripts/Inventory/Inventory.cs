using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int slots;

    private int slotsOccupied;
    private int slotSelected;
    private List<Item> array;
    private Hashtable itemMap;

    public Inventory(int size)
    {
        this.slots = size;
    }

    /// <summary>
    /// Item subclass
    /// </summary>
    public class Item
    {
        public string id;
        public int count;
        public bool stackable;
        //add assoc. sprite here eventually

        public Item(string id, int count, bool stackable = true)
        {
            if (!stackable && count > 1){
                Debug.LogWarning("This item is not stackable, yet multiple were specified.");
                count = 1;
            }
            //Debug.Log("Created an item of type " + id + ", and of count " + count.ToString());
            this.id = id;
            this.count = count;
            this.stackable = stackable;
        }
    }

    /// <summary>
    /// If an item exists, returns the index of that item. returns -1 on non-existance.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private int ItemExists(string id)
    {
        if (array == null) { Debug.LogError("Fatal Inventory Error: Inventory set to null!");}
        else
        {
            for (int i = 0; i < array.Count; i++)
            {
                Item item = array[i];
                if (item == null) { continue; }
                if (item.id == id) { return i; }
            }
        }
        return -1;
    }

    /// <summary>
    /// Linear search for a free spot in the array. Return -1 on non-existance.
    /// </summary>
    /// <returns></returns>
    private int SeekFreeSlot()
    {
        for (int i = 0; i < array.Count; i++)
        {
            Item item = array[i];
            if (item == null) { return i; }
        }
        return -1;
    }

    public int Add(string id, int count)
    {

        Debug.Log("Adding " + id);


        //does this item exist in the game?
        Item itemDefinition = FetchRefItem(id);
        if (itemDefinition == null) { return -1; }

        //does this item already exist in the inventory?
        int itemIndex = ItemExists(id);

        // not present in inventory -> create new item
        if (itemIndex < 0 )
        {
            int nextFreeSlot = SeekFreeSlot();
            if (nextFreeSlot != -1)
            {
                array[nextFreeSlot] = new Item(itemDefinition.id,itemDefinition.count, itemDefinition.stackable);
                if (itemDefinition.stackable == false) { return 1; }
            }
        }

        // present in inventory -> increment existing item
        else
        {
            if (itemDefinition.stackable == true)   // increment the number of items in this slot
            {
                array[itemIndex].count += count;
                return count;
            }
            if (itemDefinition.stackable == false)  // do not change the number of items in this slot
            {
                Debug.LogWarning("Cannot stack this item. No items were added.");
                return 0;
            }

        }

        return 0;

    }

    /// <summary>
    /// Get the item denoted by 'id' and return it, removing it from the array.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item Remove(string id)
    {
        int itemIndex = ItemExists(id);
        if (itemIndex < 0) {return null;}

        Item item = array[itemIndex];
        array[itemIndex] = null;

        return item;
    }

    /// <summary>
    /// Get the reference item associated with 'id'
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Item FetchRefItem(string id)
    {
        if (itemMap.ContainsKey(id) == false){return null;} //in C#, accessing non-existant keys gives runtime error.
        return (Item) itemMap[id];
    }

    /// <summary>
    /// Create and populate the item hashtable
    /// </summary>
    /// <returns></returns>
    private Hashtable CreateItemMap()
    {
        Hashtable map = new Hashtable();

        //import all items
        map.Add("patties", new Item("patties", 1));
        map.Add("key", new Item("key", 1, false));
        map.Add("lettuce", new Item("lettuce", 1, false));
        //Debug.Log("Added three items to the item map");
        //end import all items

        return map;
    }

    /// <summary>
    /// Create the new arraylist used to store array items
    /// </summary>
    /// <returns></returns>
    private List<Item> CreateArray()
    {
        return new List<Item>();
    }

    public void Start()
    {
        array   = CreateArray();
        itemMap = CreateItemMap();
        
    }



}
