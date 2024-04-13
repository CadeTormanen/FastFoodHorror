using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Drinks
{
    default_soda
}

public class DrinkAssembler : MonoBehaviour, Interaction
{
    [SerializeField] private GameObject drinkModel;

    public Inventory playerInventory;
    private Hashtable itemToDrinkMap;
    private bool isDrinkPlaced;
    private Vector3 drinkLocationOffset;

    public string interactionText { get; set; }
    public void ExecuteInteraction()
    {
        AddDrink(playerInventory.GetSelectedItem().id);
        playerInventory.Remove("cup_full", 1, playerInventory.slotSelected);
    } 

    public void ValidateInteraction()
    {

    } 

    public bool Possible()
    {
        ItemData.Item i = playerInventory.GetSelectedItem();
        if (i == null) return false;
        if (CanAddDrink(i.id)) return true;
        return false;
    }   
    
    public bool CanAddDrink(string id)
    {
        if (itemToDrinkMap.ContainsKey(id))
        {
            Drinks d = (Drinks) itemToDrinkMap[id];
            Debug.Log(d);
            return CanAddDrink((Drinks) itemToDrinkMap[id]);
        }
        return false;
    }

    public bool CanAddDrink(Drinks drink)
    {
        if (isDrinkPlaced) return false;
        return true;
    }

    //Add a drink. Return number of drinks added.
    public int AddDrink(Drinks drink)
    {
        if (isDrinkPlaced == true) { return 0; }
        else isDrinkPlaced = true;

        //Instantiate ingredient model on top of tray/the rest of the burger
        GameObject model = Instantiate(drinkModel, this.transform.position, Quaternion.identity);
        model.transform.position = this.transform.position + drinkLocationOffset;
        return 1;
    }

    //Add a drink. Return number of drinks added.
    public int AddDrink(string id)
    {
        if (itemToDrinkMap.ContainsKey(id))
        {
            AddDrink((Drinks)(itemToDrinkMap[id]));
            return 1;
        }
        return 0;
    }

    //Create the map that associates items with drinks.
    public Hashtable CreateItemToDrinkMap()
    {
        Hashtable map = new Hashtable();
        map.Add("cup_full",Drinks.default_soda);
        return map;
    }

    public void Start()
    {
        drinkLocationOffset = new Vector3(0.05f,0.2f,-0.2f);
        itemToDrinkMap = CreateItemToDrinkMap();
    }



}
