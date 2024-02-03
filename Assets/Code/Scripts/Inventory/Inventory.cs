using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slots;
    public GameObject itemDatabaseObject;

    private int slotsOccupied;
    private int slotSelected;
    private List<Item> array;
    private ItemData itemDatabase;

    public Inventory(int size)
    {
        this.slots = size;
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
        Item itemDefinition = itemDatabase.FetchRefItem(id);
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
    /// Get the item(s) denoted by 'id' and return all, removing them from the array.
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
    /// Get 'count' items denoted by 'id' and return them, removing those items from the array.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item Remove(string id, int count)
    {
        // Ensure the item is present
        int itemIndex = ItemExists(id);
        if (itemIndex < 0) { return null; }

        // Check if there's enough items to take 'count'
        Item item = array[itemIndex];
        int difference = item.count - count;

        // There is more than enough items: return new instance with 'count' items
        if (difference > 0)
        {
            item.count -= count;
            return new Item(id, count, true);
        }
        // There is not enough to have any left over, return the instance in the array.
        else
        {
            array[itemIndex] = null;
            return item;
        }

    }

    public void Start()
    {
        array   = new List<Item>();
        itemDatabase = itemDatabaseObject.GetComponent<ItemData>();
    }

}
