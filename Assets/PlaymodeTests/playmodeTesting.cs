using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static UnityEngine.Networking.UnityWebRequest;

public class playmodeTesting
{
    Inventory inventory;

    [SetUp]
    public void Init()
    {
        GameObject obj = new GameObject("Inventory");
        inventory = obj.AddComponent<Inventory>();
        inventory.Start();


    }

    [UnityTest]
    //ACCEPTANCE TEST 1  We should be able to add one item and retrieve it. Subsequent retrievals will present null as item no longer exists.
    public IEnumerator AddAndRemoveOneItem()
    {
        int result = inventory.Add("patty_raw");
        //Debug.Log(result);
        ItemData.Item item_first_removal = inventory.Remove("patty_raw", 1);
        ItemData.Item item_second_removal = inventory.Remove("patty_raw", 1);

        yield return null;

        Assert.AreEqual(item_first_removal.id, "patty_raw");
        Assert.AreEqual(item_second_removal, null);
    }

    [UnityTest]
    //ACCEPTANCE TEST 2  Removal from an empty inventory should yield null
    public IEnumerator RemoveFromEmpty()
    {
        ItemData.Item item_first_removal = inventory.Remove("cup_empty", 1);


        yield return null;

        Assert.AreEqual(item_first_removal, null);

    }

    [UnityTest]
    //ACCEPTANCE TEST 3 Adding to a full inventory should not be possible.
    public IEnumerator AddToFull()
    {
        int result;
        for (int i = 0; i < inventory.SlotNumber(); i++)
        {
            result = inventory.Add("cup_empty");
            Assert.AreNotEqual(result, 0);          // all adds should succeed as the inventory is not full yet.
        }

        //one add over capacity should fail.
        result = inventory.Add("cup_empty");
        Assert.AreEqual(result, 0);

        yield return null;
    }


    [UnityTest]
    //ACCEPTANCE TEST 4   We should be able to store multiple different items at once.
    public IEnumerator AddAndRemoveMultipleDifferentItems()
    {
        int result = inventory.Add("cup_empty");
        int result2 = inventory.Add("cup_full");
        int result3 = inventory.Add("patty_raw");
        int result4 = inventory.Add("patty_cooked");

        yield return null;

        //remove all added items
        ItemData.Item item1 = inventory.Remove("cup_empty", 1);
        ItemData.Item item2 = inventory.Remove("cup_full", 1);
        ItemData.Item item3 = inventory.Remove("patty_raw", 1);
        ItemData.Item item4 = inventory.Remove("patty_cooked", 1);

        //try to remove all added items again
        ItemData.Item item1_redundant = inventory.Remove("cup_empty", 1);
        ItemData.Item item2_redundant = inventory.Remove("cup_full", 1);
        ItemData.Item item3_redundant = inventory.Remove("patty_raw", 1);
        ItemData.Item item4_redundant = inventory.Remove("patty_cooked", 1);

        Assert.AreEqual(item1.id, "cup_empty");
        Assert.AreEqual(item2.id, "cup_full");
        Assert.AreEqual(item3.id, "patty_raw");
        Assert.AreEqual(item4.id, "patty_cooked");

        Assert.AreEqual(item1_redundant, null);
        Assert.AreEqual(item2_redundant, null);
        Assert.AreEqual(item3_redundant, null);
        Assert.AreEqual(item4_redundant, null);

    }

    //ACCEPTANCE TEST 5    We should be able to fill multiple slots with the same item
    [UnityTest]
    public IEnumerator AddAndRemoveMultipleEqualItems()
    {
        int result1 = inventory.Add("cup_empty");
        int result2 = inventory.Add("cup_empty");


        yield return null;

        //remove all added items
        ItemData.Item ritem1 = inventory.Remove("cup_empty", 1);
        ItemData.Item ritem2 = inventory.Remove("cup_empty", 1);
        ItemData.Item ritem3 = inventory.Remove("cup_empty", 1);

        Assert.AreEqual(ritem1.id, "cup_empty");
        Assert.AreEqual(ritem2.id, "cup_empty");
        Assert.AreEqual(ritem3, null);

    }

    //ACCEPTANCE TEST 6    We should not be able to add items that don't exist in the game.
    [UnityTest]
    public IEnumerator AddGarbageItems()
    {
        int result1 = inventory.Add("cup_asdftersasgtyewlj");
        Assert.AreEqual(result1, -1);
        ItemData.Item item1 = inventory.Remove("cup_asdftersasgtyewlj", 1);
        Assert.AreEqual(item1, null);

        yield return null;
    }

    //ACCEPTANCE TEST 7    We should not be able to add items that don't exist in the game.
    [UnityTest]
    public IEnumerator RemoveNonExistingItem()
    {
        ItemData.Item item1 = inventory.Remove("cup_asdftersasgtyewlj", 1);
        Assert.AreEqual(item1, null);

        yield return null;
    }



    /// <summary>
    /// White Box 1: 100% Statement Coverage For Add Method
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator AddAnItem()
    {
        int result;


        result = inventory.Add("not an item!");                      //Covers statements that run when item doesn't exist: (return -1)
        Assert.AreEqual(result, -1);

        for (int i = 0; i < inventory.SlotNumber(); i++)    //Covers statements that run when there are free slots
        {
            result = inventory.Add("cup_empty");
            Assert.AreNotEqual(result, -1); //not a bad item
            Assert.AreNotEqual(result, 0);  //item was added
        }

        result = inventory.Add("cup_empty");                         //Covers statemetns that run when there are not free slots
        Assert.AreEqual(result, 0); //item not added

        yield return null;
    }
    //This code covers 100% of statements in the function "Add":

    /*    public int Add(string id)
        {
            if (!ItemDataObject.IsItem(id)) { return -1; }

            // not present in inventory -> create new item
            int freeSlot = SeekFreeSlot();
            if (freeSlot != -1)
            {
                array[freeSlot] = ItemDataObject.GetNewItem(id);
                return 1;
            }
            return 0;
        }

        

        /// Linear search for a free spot in the array. Return -1 on non-existance.
        private int SeekFreeSlot()
        {
            Debug.Log(array.Count);
            for (int i = 0; i < array.Count; i++)
            {
                ItemData.Item item = array[i];
                Debug.Log(item);
                if (item == null) { return i; }
            }
            return -1;
        }

     */



    /// <summary>
    /// White Box 2: 100% statement coverage for Remove() Method.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RemoveAnItem()
    {
        inventory.Add("cup_empty");
        ItemData.Item item;
        item = inventory.Remove("cup_empty");   //covers statements where items exists
        Assert.AreEqual(item.id, "cup_empty");
        item = inventory.Remove("cup_full");    //covers statements where item doesn't exist
        Assert.AreEqual(item, null);

        yield return null;
    }

    /*    public ItemData.Item Remove(string id)
        {
            int slot = ItemExists(id);
            if (slot == -1) { return null; }
            ItemData.Item item = array[slot];
            array[slot] = null;
            return item;
        }*/

    /*   private int ItemExists(string id)
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
       }*/


    /// <summary>
    /// White Box 3: 100% statement coverage for SelectPreviousSlot() Method
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SelectPrevious()
    {
        inventory.slotSelected = 0;
        inventory.SelectPreviousSlot();
        Assert.AreEqual(inventory.SlotNumber() - 1, inventory.slotSelected);  //statements where slot is equal to 0

        inventory.slotSelected = 1;
        inventory.SelectPreviousSlot();
        Assert.AreEqual(0, inventory.slotSelected);                          //statements where slot is equal to non-0

        yield return null;
    }

    /*
    public void SelectPreviousSlot()
    {
        if (slotSelected == 0) { slotSelected = slots - 1; }
        else { slotSelected--; }
    }
*/


    /// <summary>
    /// White Box 4: 100% statement coverage for SelectNextSlot() Method
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SelectNext()
    {
        inventory.slotSelected = inventory.SlotNumber() - 1;
        inventory.SelectNextSlot();
        Assert.AreEqual(0, inventory.slotSelected);                          // statements where slot number is equal to the max slot number

        inventory.slotSelected = inventory.SlotNumber() - 2;
        inventory.SelectNextSlot();
        Assert.AreEqual(inventory.SlotNumber() - 1, inventory.slotSelected); // statements where slot number is less than the max slot number

        yield return null;
    }

    /*   
        public void SelectNextSlot()
        {
            if (slotSelected == slots - 1) { slotSelected = 0; }
            else { slotSelected++; }
        }*/


    // Integration Test
    // This test combines the INVENTORY and ItemDrop Classes in action.
    // it is big-bang integration testing.
    [UnityTest]
    public IEnumerator IntegrateDroppingItems()
    {
        inventory.Add("cup_empty");
        inventory.Add("cup_full");
        inventory.slotSelected = 1;
        GameObject dropped_cup = inventory.DropItem(inventory.slotSelected, new Vector3(0, 0, 0));
        Assert.AreEqual(dropped_cup.GetComponent<ItemDrop>().item.id, "cup_full");
        yield return null;
    }




}
