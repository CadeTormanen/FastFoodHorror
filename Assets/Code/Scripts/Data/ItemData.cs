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
    [SerializeField] private Texture2D inventoryBucketBroomTexture;

    [SerializeField] private Texture2D inventoryBucketBunTexture;
    [SerializeField] private Texture2D inventoryBucketKetchupTexture;
    [SerializeField] private Texture2D inventoryBucketMustardTexture;
    [SerializeField] private Texture2D inventoryBucketCheeseTexture;
    [SerializeField] private Texture2D inventoryBucketLettuceTexture;

    [SerializeField] private GameObject modelPattyCooked;
    [SerializeField] private GameObject modelPattyRaw;
    [SerializeField] private GameObject modelCupEmpty;
    [SerializeField] private GameObject modelCupFull;
    [SerializeField] private GameObject modelCheese;
    [SerializeField] private GameObject modelMustard;
    [SerializeField] private GameObject modelBunTop;
    [SerializeField] private GameObject modelBunBottom;
    [SerializeField] private GameObject modelKetchup;
    [SerializeField] private GameObject modelLettuce;

    public class Item
    {
        public string id;
        public string displayName;
        public int count;
        public int maxcount;
        public bool stackable;
        public Sprite sprite;
        public bool keyItem;
        public GameObject model;

        public Item(string id, int count, int maxcount, Sprite sprite, string dname, bool stackable = true, bool keyItem = false, GameObject model = null)
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
            this.displayName = dname;
        }
    }

    private Hashtable CreateItemMap()
    {
        Hashtable map = new Hashtable();

        Sprite pattyRawSprite      = Sprite.Create(inventoryBucketPattyRawTexture, new Rect(0, 0, inventoryBucketPattyRawTexture.width, inventoryBucketPattyRawTexture.height), Vector2.zero);
        Sprite pattyCookedSprite   = Sprite.Create(inventoryBucketPattyCookedTexture, new Rect(0, 0, inventoryBucketPattyCookedTexture.width, inventoryBucketPattyCookedTexture.height), Vector2.zero);
        Sprite cupEmptySprite      = Sprite.Create(inventoryBucketCupEmptyTexture, new Rect(0, 0, inventoryBucketCupEmptyTexture.width, inventoryBucketCupEmptyTexture.height), Vector2.zero);
        Sprite cupFullSprite       = Sprite.Create(inventoryBucketCupFullTexture, new Rect(0, 0, inventoryBucketCupFullTexture.width, inventoryBucketCupFullTexture.height), Vector2.zero);
        Sprite broomSprite         = Sprite.Create(inventoryBucketBroomTexture, new Rect(0, 0, inventoryBucketBroomTexture.width, inventoryBucketBroomTexture.height), Vector2.zero);
        Sprite keySprite           = Sprite.Create(inventoryBucketKeyTexture, new Rect(0, 0, inventoryBucketKeyTexture.width, inventoryBucketKeyTexture.height), Vector2.zero);
        Sprite ketchupSprite       = Sprite.Create(inventoryBucketKetchupTexture, new Rect(0, 0, inventoryBucketKetchupTexture.width, inventoryBucketKetchupTexture.height), Vector2.zero);
        Sprite mustardSprite       = Sprite.Create(inventoryBucketMustardTexture, new Rect(0, 0, inventoryBucketMustardTexture.width, inventoryBucketMustardTexture.height), Vector2.zero);
        Sprite cheeseSprite        = Sprite.Create(inventoryBucketCheeseTexture, new Rect(0, 0, inventoryBucketCheeseTexture.width, inventoryBucketCheeseTexture.height), Vector2.zero);
        Sprite lettuceSprite       = Sprite.Create(inventoryBucketLettuceTexture, new Rect(0, 0, inventoryBucketLettuceTexture.width, inventoryBucketLettuceTexture.height), Vector2.zero);
        Sprite bunSprite           = Sprite.Create(inventoryBucketBunTexture, new Rect(0, 0, inventoryBucketBunTexture.width, inventoryBucketBunTexture.height), Vector2.zero);
        Sprite emptySprite         = Sprite.Create(inventoryBucketEmptyTexture, new Rect(0, 0, inventoryBucketEmptyTexture.width, inventoryBucketEmptyTexture.height), Vector2.zero);
<<<<<<< HEAD
<<<<<<< HEAD
        Sprite fryRawSprite        = Sprite.Create(inventoryBucketFryRawTexture, new Rect(0, 0, inventoryBucketFryRawTexture.width, inventoryBucketFryRawTexture.height), Vector2.zero);
        Sprite fryCookedSprite     = Sprite.Create(inventoryBucketFryCookedTexture, new Rect(0, 0, inventoryBucketFryCookedTexture.width, inventoryBucketFryCookedTexture.height), Vector2.zero);
            
        //name(string)   count(int)   max-count(int)   sprite(Sprite)   stackable(bool)?   keyitem(bool)?   model(gameobject)?
        map.Add("patty_raw",      new Item("patty_raw", 1, 12,pattyRawSprite, "Raw Patty", false, false,modelPattyRaw));
        map.Add("patty_cooked",   new Item("patty_cooked", 1, 12,pattyCookedSprite, "Cooked Patty", false, false, modelPattyCooked));
        map.Add("cup_empty",      new Item("cup_empty", 1, 12,cupEmptySprite, "Empty Cup", false, false,modelCupEmpty));
        map.Add("cup_full",       new Item("cup_full", 1, 12,cupFullSprite, "Soda", false, false, modelCupFull));
        map.Add("key",            new Item("key", 1, 1,keySprite, "Key", false, true, null));
        map.Add("empty",          new Item("empty", 1, 1,emptySprite, "EMPTY",false, false));
        map.Add("broom",          new Item("broom", 1, 1,broomSprite, "Broom", false, false,null));
        map.Add("bun_top",        new Item("bun_top", 1, 1,bunSprite, "Top Bun", false, false, modelBunTop));
        map.Add("bun_bottom",     new Item("bun_bottom", 1, 1,bunSprite, "Bottom Bun", false, false, modelBunBottom));
        map.Add("bun",            new Item("bun", 1, 1,bunSprite, "Bun", false, false, modelBunBottom));
        map.Add("cheese",         new Item("cheese", 1, 1,cheeseSprite, "Cheese", false, false, modelCheese));
        map.Add("ketchup",        new Item("ketchup", 1, 1,ketchupSprite, "Ketchup", false, false, modelKetchup));
        map.Add("mustard",        new Item("mustard", 1, 1,mustardSprite, "Mustard", false, false, modelMustard));
        map.Add("lettuce",        new Item("lettuce", 1, 1,lettuceSprite, "Lettuce", false, false,modelLettuce));
        map.Add("french_fry_raw", new Item("french_fry_raw", 1, 1, fryRawSprite, "Raw Fries",false, false,modelFryRaw));
        map.Add("french_fry_done",new Item("french_fry_done", 1, 1, fryCookedSprite, "French Fry",false, false,modelFryCooked));
=======
=======
>>>>>>> parent of 2cb752b8 (Incorporate 'Cade/Pathing' paths, Create customers, register)
                                           //name(string)   count(int)   max-count(int)   sprite(Sprite)   stackable(bool)?   keyitem(bool)?   model(gameobject)?
        map.Add("patty_raw",      new Item("patty_raw", 1, 12,pattyRawSprite, false, false,modelPattyRaw));
        map.Add("patty_cooked",   new Item("patty_cooked", 1, 12,pattyCookedSprite, false, false, modelPattyCooked));
        map.Add("cup_empty",      new Item("cup_empty", 1, 12,cupEmptySprite, false, false,modelCupEmpty));
        map.Add("cup_full",       new Item("cup_full", 1, 12,cupFullSprite, false, false, modelCupFull));
        map.Add("key",            new Item("key", 1, 1,keySprite, false, true));
        map.Add("empty",          new Item("empty", 1, 1,emptySprite, false, false));
        map.Add("broom",          new Item("broom", 1, 1,broomSprite, false, false));
        map.Add("bun_top",        new Item("bun_top", 1, 1,bunSprite, false, false, modelBunTop));
        map.Add("bun_bottom",     new Item("bun_bottom", 1, 1,bunSprite, false, false, modelBunBottom));
        map.Add("cheese",         new Item("cheese", 1, 1,cheeseSprite, false, false, modelCheese));
        map.Add("ketchup",        new Item("ketchup", 1, 1,ketchupSprite, false, false, modelKetchup));
        map.Add("mustard",        new Item("mustard", 1, 1,mustardSprite, false, false, modelMustard));
        map.Add("lettuce",        new Item("lettuce", 1, 1,lettuceSprite, false, false,modelLettuce));
<<<<<<< HEAD
>>>>>>> parent of 2cb752b8 (Incorporate 'Cade/Pathing' paths, Create customers, register)
=======
>>>>>>> parent of 2cb752b8 (Incorporate 'Cade/Pathing' paths, Create customers, register)

        return map;
    }

    // Create the item called 'id'
    public Item GetNewItem(string id)
    {
        if (itemInfoMap.ContainsKey(id) == false) { return null; } //in C#, accessing non-existant keys gives runtime error.
        Item refItem = (Item) itemInfoMap[id];
        Item newItem = new Item(refItem.id, 1, refItem.maxcount, refItem.sprite,refItem.displayName, refItem.stackable, refItem.keyItem,refItem.model);
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






