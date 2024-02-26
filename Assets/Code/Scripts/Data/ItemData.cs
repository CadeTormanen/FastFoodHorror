using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{

    private Hashtable itemInfoMap;

    [SerializeField] private Texture2D inventoryBucketEmptyTexture;
    [SerializeField] private Texture2D inventoryBucketKeyTexture;
    [SerializeField] private Texture2D inventoryBucketPattyRawTexture;
    [SerializeField] private Texture2D inventoryBucketPattyCookedTexture;
    [SerializeField] private Texture2D inventoryBucketCupEmptyTexture;
    [SerializeField] private Texture2D inventoryBucketCupFullTexture;

    [SerializeField] private GameObject modelPattyCooked;
    [SerializeField] private GameObject modelPattyRaw;
    [SerializeField] private GameObject modelCupEmpty;
    [SerializeField] private GameObject modelCupFull;

    public class Item
    {
        public string id;
        public int count;
        public int maxcount;
        public bool stackable;
        public Sprite sprite;
        public bool keyItem;
        public GameObject model;

        public Item(string id, int count, int maxcount, Sprite sprite, bool stackable = true, bool keyItem = false, GameObject model = null)
        {
            if (!stackable && count > 1)
            {
                Debug.LogWarning("This item is not stackable, yet multiple were specified.");
                count = 1;
            }

            this.sprite     = sprite;
            this.id         = id;
            this.count      = count;
            this.stackable  = stackable;
            this.maxcount   = maxcount;
            this.keyItem    = keyItem;
            this.model      = model;
        }
    }

    private Hashtable CreateItemMap()
    {
        Hashtable map = new Hashtable();

        Sprite pattyRawSprite       = Sprite.Create(inventoryBucketPattyRawTexture, new Rect(0, 0, inventoryBucketPattyRawTexture.width, inventoryBucketPattyRawTexture.height), Vector2.zero);
        Sprite pattyCookedSprite    = Sprite.Create(inventoryBucketPattyCookedTexture, new Rect(0, 0, inventoryBucketPattyCookedTexture.width, inventoryBucketPattyCookedTexture.height), Vector2.zero);
        Sprite cupEmptySprite       = Sprite.Create(inventoryBucketCupEmptyTexture, new Rect(0, 0, inventoryBucketCupEmptyTexture.width, inventoryBucketCupEmptyTexture.height), Vector2.zero);
        Sprite cupFullSprite        = Sprite.Create(inventoryBucketCupFullTexture, new Rect(0, 0, inventoryBucketCupFullTexture.width, inventoryBucketCupFullTexture.height), Vector2.zero);
        Sprite keySprite            = Sprite.Create(inventoryBucketKeyTexture, new Rect(0, 0, inventoryBucketKeyTexture.width, inventoryBucketKeyTexture.height), Vector2.zero);
        Sprite emptySprite          = Sprite.Create(inventoryBucketEmptyTexture, new Rect(0, 0, inventoryBucketEmptyTexture.width, inventoryBucketEmptyTexture.height), Vector2.zero);

        map.Add("patty_raw",    new Item("patty_raw", 1, 12,    pattyRawSprite, false, false,modelPattyRaw));
        map.Add("patty_cooked", new Item("patty_cooked", 1, 12, pattyCookedSprite, false, false, modelPattyCooked));
        map.Add("cup_empty",    new Item("cup_empty", 1, 12,    cupEmptySprite, false, false,modelCupEmpty));
        map.Add("cup_full",     new Item("cup_full", 1, 12,     cupFullSprite, false, false, modelCupFull));
        map.Add("key",          new Item("key", 1, 1,           keySprite, false, true));
        map.Add("empty",        new Item("empty", 1, 1,         emptySprite, false, false));

        return map;
    }

    // Create the item called 'id'
    public Item GetNewItem(string id)
    {
        if (itemInfoMap.ContainsKey(id) == false) { return null; } //in C#, accessing non-existant keys gives runtime error.
        Item refItem = (Item) itemInfoMap[id];
        Item newItem = new Item(refItem.id, 1, refItem.maxcount, refItem.sprite, refItem.stackable, refItem.keyItem,refItem.model);
        return newItem;
    }

    // Get the inventory thumbnail sprite associated with the item called 'id'
    public Sprite GetInventoryThumbnail(string id)
    {
        if (itemInfoMap.ContainsKey(id) == false) { return null; }
        Item refItem = (Item)itemInfoMap[id];
        return refItem.sprite;
    }

    public bool GetKeyItemStatus(string id)
    {
        if (itemInfoMap.ContainsKey(id) == false) { return false; }
        Item refItem = (Item)itemInfoMap[id];
        return refItem.keyItem;
    }

    public void Start()
    {
        itemInfoMap = CreateItemMap();
    }

}






