using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int slots;
    [SerializeField] private GameObject HUDAnchorObject;
    [SerializeField] private GameObject HUDCanvas;
    [SerializeField] private Texture2D inventoryBucketEmptyTexture;
    [SerializeField] private Texture2D inventoryBucketKeyTexture;
    [SerializeField] private Texture2D inventoryBucketPattyRawTexture;
    [SerializeField] private Texture2D inventoryBucketPattyCookedTexture;
    [SerializeField] private Texture2D inventoryBucketCupEmptyTexture;
    [SerializeField] private Texture2D inventoryBucketCupFullTexture;
    [SerializeField] private Texture2D inventoryBucketHighlightSelectionTexture;

    public int slotWidth;
    public int slotHeight;
    public int slotPadding;
    public int slotSelected;

    private Hashtable itemMap;                // stores stats of each item
    private List<Item> array;                 // stores the actual items 
    private GameObject[] invSlotObjectsArray; // stores all the inventory slot objects
    private GameObject invHighlightObject;    // stores the inventory object that highlights the selected slot
    private int slotsOccupied;
    

    #region Inventory Data

    /// Get the reference item associated with 'id'
    public Item FetchItemReference(string id)
    {
        if (itemMap.ContainsKey(id) == false) { return null; } //in C#, accessing non-existant keys gives runtime error.
        return (Item)itemMap[id];
    }

    public class Item
    {
        public string id;
        public int count;
        public int maxcount;
        public bool stackable;
        public Sprite sprite;
        public bool keyItem;

        public Item(string id, int count, int maxcount, Sprite sprite, bool stackable = true, bool keyItem = false)
        {
            if (!stackable && count > 1)
            {
                Debug.LogWarning("This item is not stackable, yet multiple were specified.");
                count = 1;
            }

            this.sprite = sprite;
            this.id = id;
            this.count = count;
            this.stackable = stackable;
            this.maxcount = maxcount;
            this.keyItem = keyItem;
        }
    }

    /// Create and populate the item reference hashtable
    private Hashtable CreateItemMap()
    {
        Hashtable map = new Hashtable();

        Sprite pattyRawSprite    = Sprite.Create(inventoryBucketPattyRawTexture, new Rect(0, 0, inventoryBucketPattyRawTexture.width, inventoryBucketPattyRawTexture.height), Vector2.zero);
        Sprite pattyCookedSprite = Sprite.Create(inventoryBucketPattyCookedTexture, new Rect(0, 0, inventoryBucketPattyCookedTexture.width, inventoryBucketPattyCookedTexture.height), Vector2.zero);
        Sprite cupEmptySprite    = Sprite.Create(inventoryBucketCupEmptyTexture, new Rect(0, 0, inventoryBucketCupEmptyTexture.width, inventoryBucketCupEmptyTexture.height), Vector2.zero);
        Sprite cupFullSprite = Sprite.Create(inventoryBucketCupFullTexture, new Rect(0, 0, inventoryBucketCupFullTexture.width, inventoryBucketCupFullTexture.height), Vector2.zero);
        Sprite keySprite = Sprite.Create(inventoryBucketKeyTexture, new Rect(0, 0, inventoryBucketKeyTexture.width, inventoryBucketKeyTexture.height), Vector2.zero);
        Sprite emptySprite = Sprite.Create(inventoryBucketEmptyTexture, new Rect(0, 0, inventoryBucketEmptyTexture.width, inventoryBucketEmptyTexture.height), Vector2.zero);

        map.Add("patty_raw", new Item("patty_raw", 1, 12, pattyRawSprite, false,false));
        map.Add("patty_cooked", new Item("patty_cooked", 1, 12, pattyCookedSprite, false, false));
        map.Add("cup_empty", new Item("cup_empty", 1, 12, cupEmptySprite, false, false));
        map.Add("cup_full", new Item("cup_full", 1, 12, cupFullSprite, false, false));
        map.Add("key", new Item("key", 1, 1, keySprite, false,true));
        map.Add("empty", new Item("empty", 1, 1, emptySprite, false, false));

        return map;
    }
    #endregion

    #region UI

    private GameObject CreateInventorySlot(string id)
    {
        Sprite emptySlotSprite = FetchItemReference("empty").sprite;
        GameObject slot = new GameObject(id);
        slot.transform.SetParent(HUDCanvas.transform);

        Image img = slot.AddComponent<Image>();
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        img.sprite = Sprite.Create(inventoryBucketEmptyTexture, new Rect(0, 0, inventoryBucketEmptyTexture.width, inventoryBucketEmptyTexture.height), Vector2.zero);
        img.sprite = emptySlotSprite;

        return slot;
    }

    /// Create Inventory Slot Images And Assign Them to the HUD Canvas.
    /// Then return an array containing each slot in order.
    private GameObject[] CreateInventoryHUD()
    {
        GameObject[] slotArray = new GameObject[slots];

        //create the highlighter object
        invHighlightObject = CreateInventorySlot("selection highlight");
        Vector3 translation = new Vector3(HUDAnchorObject.transform.position.x, HUDAnchorObject.transform.position.y, 0);
        //Vector3 translation = new Vector3(500, 50, 0);
        invHighlightObject.GetComponent<Image>().sprite = Sprite.Create(inventoryBucketHighlightSelectionTexture, new Rect(0, 0, inventoryBucketHighlightSelectionTexture.width, inventoryBucketHighlightSelectionTexture.height), Vector2.zero);
        invHighlightObject.GetComponent<Image>().transform.position = translation;


        for (int i = 0; i < this.slots; i++)
        {
            //create the slot object
            slotArray[i] = CreateInventorySlot("slot" + i.ToString());

            //place the inventory slot object into it's proper location on the canvas
            translation = new Vector3(HUDAnchorObject.transform.position.x + slotWidth * i + slotPadding * i, HUDAnchorObject.transform.position.y, 0);
            slotArray[i].GetComponent<Image>().transform.Translate(translation);
        }

        invHighlightObject.transform.SetAsLastSibling();
        return slotArray;
    }

    //update the sprite for each inventory slot
    public void UpdateInventoryImage()
    {
        //update slot images
        for (int i = 0; i < this.slots; i++)
        {
            Item item = array[i];
            if (item != null)
            {
                invSlotObjectsArray[i].GetComponent<Image>().sprite = item.sprite;
            }
            else
            {
                invSlotObjectsArray[i].GetComponent<Image>().sprite = FetchItemReference("empty").sprite;
            }
        }

        //update highlighter location
        invHighlightObject.GetComponent<Image>().transform.position = new Vector3(HUDAnchorObject.transform.position.x + slotSelected * slotWidth + slotSelected * slotPadding, HUDAnchorObject.transform.position.y, 0);
    }

    #endregion

    #region Helper Functions
    /// If an item exists, returns the index of that item. returns -1 on non-existance.
    private int ItemExists(string id)
    {
        if (array == null) { Debug.LogError("Fatal Inventory Error: Inventory set to null!"); }
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


    /// Linear search for a free spot in the array. Return -1 on non-existance.
    private int SeekFreeSlot()
    {
        for (int i = 0; i < array.Count; i++)
        {
            Item item = array[i];
            if (item == null) { return i; }
        }
        return -1;
    }

    #endregion

    #region Interface Functions
    /// Add "count" of an item to the inventory
    public int Add(string id, int count)
    {
        Debug.Log("Adding " + id);

        //does this item exist in the game?
        Item itemDefinition = FetchItemReference(id);
        if (itemDefinition == null) { return -1; }

        //does this item already exist in the inventory?
        int itemIndex = ItemExists(id);

        // not present in inventory -> create new item
        if (itemIndex < 0)
        {
            int nextFreeSlot = SeekFreeSlot();
            if (nextFreeSlot != -1)
            {
                array[nextFreeSlot] = new Item(itemDefinition.id, 1, itemDefinition.maxcount, itemDefinition.sprite, itemDefinition.stackable);
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
                int freeSlot = SeekFreeSlot();
                if (freeSlot != -1)
                {
                    array[freeSlot] = new Item(itemDefinition.id, 1, itemDefinition.maxcount, itemDefinition.sprite, itemDefinition.stackable);
                }
                return 0;
            }

        }
        return 0;
    }

    public int Add(string id, int count, int slot)
    {
        if ((slot < 0) || (slot >= slots)){ return -1; }

        //does this item exist in the game?
        Item itemDefinition = FetchItemReference(id);
        if (itemDefinition == null) { return -1; }

        if (array[slot] == null)
        {
            array[slot] = new Item(itemDefinition.id, count, itemDefinition.maxcount, itemDefinition.sprite, itemDefinition.stackable);
            return count;
        }

        return 0;
    }


    public Item GetSelectedItem()
    {
        return array[slotSelected];
    }

    public bool ItemPresent(string id)
    {
        if (ItemExists(id) != -1)
        {
            return true;
        }
        return false;
    }

    /// REMOVE all instances of an item from the inventory
    public Item Remove(string id)
    {
        int itemIndex = ItemExists(id);
        if (itemIndex < 0) { return null; }

        Item item = array[itemIndex];
        array[itemIndex] = null;

        return item;
    }

    /// REMOVE "count" instances of an item from the inventory
    public Item Remove(string id, int count)
    {
        // Ensure the item is present in inventory
        int itemIndex = ItemExists(id);
        if (itemIndex < 0) { return null; }

        // Check if there's enough items to take 'count' items from the inventory
        Item item = array[itemIndex];
        int difference = item.count - count;

        // There is more than enough items: return new instance with 'count' items
        if (difference > 0)
        {
            item.count -= count;
            return new Item(id, count, item.maxcount, item.sprite, item.stackable);
        }
        // There is not enough to have any left over, return all of te items in the array
        else
        {
            array[itemIndex] = null;
            return item;
        }
    }

    public Item Remove(string id, int count, int slot)
    {
        Item item = array[slot];
        if (item == null) { return null; }
        if ((slot >= slots) || (slot < 0)) {return null; }

        if (item.count <= count)
        {
            array[slot] = null;
            return item;
        }
        else
        {
            item.count -= count;
            return new Item(item.id,count,item.maxcount,item.sprite, item.stackable);
        }
    }


    /// Go to Next Slot
    public void SelectNextSlot()
    {
        if (slotSelected == slots - 1) { slotSelected = 0; }
        else { slotSelected++; }
    }


    /// Go to Previous Slot
    public void SelectPreviousSlot()
    {
        if (slotSelected == 0) { slotSelected = slots - 1; }
        else { slotSelected--; }
    }


    public bool IsKeyItem(string id)
    {
        Item item = this.FetchItemReference(id);
        if (item == null) { return false; }
        return item.keyItem; 
    }

    #endregion

    public void Update()
    {
        UpdateInventoryImage();
    }

    public void Start()
    {
        array = new List<Item>();
        for (int i = 0; i < slots; i++) { array.Add(null); }
        itemMap = CreateItemMap();
        invSlotObjectsArray = CreateInventoryHUD();
    }
}