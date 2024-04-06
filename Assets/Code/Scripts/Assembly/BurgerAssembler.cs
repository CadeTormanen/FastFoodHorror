using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingredients
{
    NOTHING,
    LOWER_BUN,
    PATTY,
    CHEESE,
    LETTUCE,
    KETCHUP,
    MUSTARD,
    UPPER_BUN
}

public class BurgerAssembler : MonoBehaviour, Interaction
{
    public Inventory playerInventory;

    private Hashtable PrereqMap;
    private Hashtable itemToIngredientMap;

    private ArrayList PlacedIngredients;
    private GameObject[] IngredientModels;
    private bool[] Burger;

    //These instance variables handles where the burger is placed relative to the tray.
    private Vector3 currentIngredientLocation;
    private Vector3 currentBurgerLocation;
    private float burgerHeightOffset = 0.04f;
    private float ingredientOffset   = 0.7f;

    //Gameobjects that are created when ingredients are added.
    private GameObject no_model;
    [SerializeField] private GameObject lower_bun_model;
    [SerializeField] private GameObject patty_model;
    [SerializeField] private GameObject cheese_model;
    [SerializeField] private GameObject lettuce_model;
    [SerializeField] private GameObject ketchup_model;
    [SerializeField] private GameObject mustard_model;
    [SerializeField] private GameObject upper_bun_model;

    #region Interaction Interface Implementation

    public string interactionText { get; set; }
    
    //add the currently equipped ingredient to this burger, and remove it from the inventory.
    public void ExecuteInteraction()
    {
        ItemData.Item item = (playerInventory.GetSelectedItem());
        Debug.Log(Add(item.id));
        playerInventory.Remove(item.id, 1, playerInventory.slotSelected);
    }

    public void ValidateInteraction()
    {

    }

    //Is it possbile to add the currently equipped ingredient to this burger?
    public bool Possible()
    {
        ItemData.Item item = (playerInventory.GetSelectedItem()) ;
        if (item == null) { return false; }
        Debug.Log(item.id);
        if (CanAdd(item.id) == true) return true;
        return false;
    }            

    #region Burger Creation Methods
    //Creates a map that can be referenced for ingredient dependencies.
    //for example, this tells us we need a lower bun before we can put on a top bun.
    private Hashtable CreateDependencyMap()
    {
        Hashtable map = new Hashtable();
        map.Add(Ingredients.LOWER_BUN, Ingredients.NOTHING);
        map.Add(Ingredients.PATTY, Ingredients.LOWER_BUN);
        map.Add(Ingredients.CHEESE, Ingredients.LOWER_BUN);
        map.Add(Ingredients.LETTUCE, Ingredients.LOWER_BUN);
        map.Add(Ingredients.KETCHUP, Ingredients.LOWER_BUN);
        map.Add(Ingredients.MUSTARD, Ingredients.LOWER_BUN);
        map.Add(Ingredients.UPPER_BUN, Ingredients.LOWER_BUN);
        return map;
    }


    private Hashtable CreateItemToIngredientMap()
    {
        Hashtable map = new Hashtable();
        map.Add("patty_cooked", Ingredients.PATTY);
        map.Add("lettuce", Ingredients.LETTUCE);
        map.Add("ketchup", Ingredients.KETCHUP);
        map.Add("mustard", Ingredients.MUSTARD);
        map.Add("cheese", Ingredients.CHEESE);
        map.Add("buns", Ingredients.LOWER_BUN);
       // map.Add("bun", Ingredients.UPPER_BUN);

        return map;
    }

    // Creates a burger array.
    // A burger consists of an array of ingredients,
    // where true/false denotes their prescence on the burger.
    private bool[] CreateEmptyBurger()
    {
        int ingredientCount = Enum.GetNames(typeof(Ingredients)).Length;
        bool[] burger = new bool[ingredientCount];
        for (int i = 0; i < ingredientCount; i++){burger[i] = false;}
        
        burger[0] = true;   //nothing is always on the burger :)

        return burger;
    }

    // Compiles array of models,
    // where models can be accessed according to their 'Ingredients' enumerated value.
    private GameObject[] GetIngredientModels()
    {
        GameObject[] models =
        {
            no_model,
            lower_bun_model,
            patty_model,
            cheese_model,
            lettuce_model,
            ketchup_model,
            mustard_model,
            upper_bun_model
        };
        return models;
    }
    #endregion

    public bool ContainsIngredient(Ingredients ingredient)
    {
        return Burger[(int)ingredient];
    }

    // If the prerequisites for an item have been met, add the item.
    // No changes are made if ingredient already exists on burger, or prereqs not met.
    // Returns 1 upon successful addition. Returns 0 on failed addition.
    public int Add(Ingredients ingredient)
    {
        if (this.Burger[(int)ingredient] == true) return 0;

        Ingredients prereq     = (Ingredients) PrereqMap[ingredient];
        bool prereqMet         = this.Burger[(int) prereq];

        if (prereqMet == true)
        {
            // Put Item on burger
            this.Burger[ (int) ingredient] = true;         
            
            //Instantiate ingredient model on top of tray/the rest of the burger
            GameObject model = Instantiate(IngredientModels[(int)ingredient], currentIngredientLocation, Quaternion.identity);
            float modelHeight = IngredientModels[(int)ingredient].transform.localScale.y;
            model.transform.position = currentIngredientLocation;
            
            //save the model so we have a handle on it.
            PlacedIngredients.Add(model);   

            //Update location to place next ingredient
            currentIngredientLocation = new Vector3(
                currentBurgerLocation.x,
                currentIngredientLocation.y + modelHeight + modelHeight * ingredientOffset,
                currentBurgerLocation.z
            );

            return 1;

        }
        return 0;
    }

    // If the prerequisites for an item have been met, add the item.
    // No changes are made if ingredient already exists on burger, or prereqs not met.
    // Returns 1 upon successful addition. Returns 0 on failed addition.
    public int Add(String id)
    {
        if (!itemToIngredientMap.ContainsKey(id)) return 0;
        else return Add((Ingredients)itemToIngredientMap[id]);
    }

    //Can we add this ingredient?
    public bool CanAdd(Ingredients ingredient)
    {
        if (ContainsIngredient(ingredient)) return false;

        Ingredients prereq = (Ingredients) PrereqMap[ingredient];
        bool prereqMet = this.Burger[(int)prereq];
        if (prereqMet == true) return true;

        return false;
    }

    //Can we add this ingredient?
    public bool CanAdd(String id)
    {
        if (!itemToIngredientMap.ContainsKey(id)) return false;
        else return CanAdd((Ingredients)itemToIngredientMap[id]);
    }

    //Clears the burger from the tray.
    public void DeleteBurger()
    {
        foreach(var model in PlacedIngredients){ Destroy((GameObject) model);}
        Burger = CreateEmptyBurger();
        currentBurgerLocation = new Vector3(transform.position.x,
                                    transform.position.y + burgerHeightOffset,
                                    transform.position.z
                                    );

        currentIngredientLocation = currentBurgerLocation;
    }



    //It is possible to pass this thing off as a burger (AKA, sell it)
    public bool ValidBurger()
    {
        if (ContainsIngredient(Ingredients.LOWER_BUN) &&
            ContainsIngredient(Ingredients.UPPER_BUN))
        {
            return true;
        }
        return false;
    }

    #endregion

    void Start()
    {
        currentBurgerLocation = new Vector3(transform.position.x,
                                            transform.position.y + burgerHeightOffset,
                                            transform.position.z
                                            );

        currentIngredientLocation = currentBurgerLocation;

        PrereqMap           = CreateDependencyMap();
        itemToIngredientMap = CreateItemToIngredientMap();
        Burger              = CreateEmptyBurger();
        IngredientModels    = GetIngredientModels();
    }
}