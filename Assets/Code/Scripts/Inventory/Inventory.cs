using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int slots;
    [SerializeField] private GameObject HUDAnchorObject;
    [SerializeField] private GameObject HUDCanvas;
    [SerializeField] private ItemData ItemDataObject;
    [SerializeField] private Texture2D inventoryBucketHighlightSelectionTexture;

    public int slotWidth;
    public int slotHeight;
    public int slotPadding;
    public int slotSelected;

    private Hashtable itemMap;                // stores stats of each item
    private List<ItemData.Item> array;                 // stores the actual items 
    private GameObject[] invSlotObjectsArray; // stores all the inventory slot objects
    private GameObject invHighlightObject;    // stores the inventory object that highlights the selected slot
    private int slotsOccupied;
   
    #region UI

    private GameObject CreateInventorySlot(string name)
    {
        Sprite emptySlotSprite = ItemDataObject.GetInventoryThumbnail("empty");
        GameObject slot = new GameObject(name);
        slot.transform.SetParent(HUDCanvas.transform);
        Image img = slot.AddComponent<Image>();
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
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
            ItemData.Item item = array[i];
            if (item != null)
            {
                invSlotObjectsArray[i].GetComponent<Image>().sprite = item.sprite;
            }
            else
            {
                invSlotObjectsArray[i].GetComponent<Image>().sprite = ItemDataObject.GetInventoryThumbnail("empty");
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
                ItemData.Item item = array[i];
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
            ItemData.Item item = array[i];
            if (item == null) { return i; }
        }
        return -1;
    }

    #endregion

    #region Interface Functions
    /// Add "count" of an item to the inventory
    public int Add(string id, int count)
    {
        //does this item already exist in the inventory?
        int itemIndex = ItemExists(id);

        // not present in inventory -> create new item
        if (itemIndex < 0)
        {
            int nextFreeSlot = SeekFreeSlot();
            if (nextFreeSlot != -1)
            {
                array[nextFreeSlot] = ItemDataObject.GetNewItem(id);
                return 1;
            }
        }

        // present in inventory -> increment existing item
        else
        {
            int freeSlot = SeekFreeSlot();
            if (freeSlot != -1)
            {
                array[freeSlot] = ItemDataObject.GetNewItem(id);
                return 1;
            }
        }
        return 0;
    }

    public int Add(string id, int count, int slot)
    {
        if ((slot < 0) || (slot >= slots)){ return -1; }

        if (array[slot] == null)
        {
            array[slot] = ItemDataObject.GetNewItem(id);
            return count;
        }

        return 0;
    }


    public ItemData.Item GetSelectedItem()
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

    public ItemData.Item Remove(string id, int count, int slot)
    {
        ItemData.Item item = array[slot];
        if (item == null) { return null; }
        if ((slot >= slots) || (slot < 0)) {return null; }
        array[slot] = null;
        return item;
    }

    public void DropItem(int slot, Vector3 whereAt)
    {
        if (slot == -1) { slot = slotSelected; }
        ItemData.Item item = array[slot];
        if (item == null) { return; }
        GameObject dropObject       = Instantiate(item.model, whereAt, Quaternion.identity, transform);
        ItemDrop itemDrop           = dropObject.AddComponent<ItemDrop>();
        itemDrop.Bind(item);
        array[slot] = null;
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
        return ItemDataObject.GetKeyItemStatus(id);
    }

    #endregion

    public void Update()
    {
        UpdateInventoryImage();
    }

    public void Start()
    {
        array = new List<ItemData.Item>();
        for (int i = 0; i < slots; i++) { array.Add(null); }
        invSlotObjectsArray = CreateInventoryHUD();
    }
}