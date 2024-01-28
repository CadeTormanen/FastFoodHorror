using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        if (!stackable && count > 1)
        {
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
/// Class for storing data about different items.
/// </summary>
public class ItemData : MonoBehaviour
{
    private Hashtable itemMap;

    /// <summary>
    /// Get the reference item associated with 'id'
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item FetchRefItem(string id)
    {
        if (itemMap.ContainsKey(id) == false) { return null; } //in C#, accessing non-existant keys gives runtime error.
        return (Item)itemMap[id];
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


        void Start()
        {
            itemMap = CreateItemMap();
        }
    }
