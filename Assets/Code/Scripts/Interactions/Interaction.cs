using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Patty
{
    private float timeRequired;
    private float timeElapsed;
    public bool done;

    public Patty(float timeRequired)
    {
        this.timeRequired = timeRequired;
        this.timeElapsed  = 0;
        this.done         = false;
    }
    public void Cook() { timeElapsed += Time.deltaTime; }
    public bool Done() { return (timeElapsed >= timeRequired); }
}

public class Grill
{
    public int pattyCount;
    public int pattyCapacity;
    private float pattyCookTime;
    private Patty[] pattyList;
    private GameObject[] pattyObjList;

    public Grill(int pattyCookTime, GameObject pattyPrefab){
        this.pattyCapacity = 4;
        this.pattyCookTime = pattyCookTime;
        this.pattyCount    = 0;
        pattyList = new Patty[pattyCapacity];
        for (int i = 0; i < pattyCapacity; i++)
        {
            pattyObjList[i] = GameObject.Instantiate(pattyPrefab);
        }



    }

    public int AddPatty()
    {
        if (pattyCount == pattyCapacity ){ Debug.Log("Failed to add patty - beyond capacity"); return -1; }
        for (int i = 0; i < pattyCapacity; i++)
        {
            if (pattyList[i] == null){
                pattyList[i] = new Patty(pattyCookTime);
                pattyCount++;
                return 0;
            }
        }
        return 0;
    }

    //Remove a cooked patty from the grill
    public bool RemovePatty()
    {
        if (pattyCount == 0) { return false; }
        for (int i =0; i < pattyList.Length; i++)
        {
            if ((pattyList[i] != null) && (pattyList[i].done == true)){
                pattyList[i] = null;
                pattyCount--;
                return true;
            }

        }
        return false;
    }

    //Loop through and cook all patties for a duration of 'deltaTime'
    public void CookPatties()
    {
        //Debug.Log("cooking patties:");
        for (int i = 0; i < pattyList.Length; i++)
        {
            Patty patty = pattyList[i];
            if (patty != null)
            {
                patty.Cook();
                if (patty.Done())
                {
                    Debug.Log("a patty is done - " + i.ToString());
                    patty.done = true;
                }
            }

        }
    }

    //is there free space on the grill?
    public bool HasCapacity()
    {
        return (pattyCount != pattyCapacity);
    }

    //are there done patties on the grill?
    public bool HasDonePatties()
    {
        for (int i = 0; i < pattyList.Length; i++)
        {
            if ((pattyList[i] != null) && (pattyList[i].done)){
                return true;
            }
        }
        return false;
    }
}

public class Interaction : MonoBehaviour
{

    public Inventory playerInventoryReference;
    public GameObject pattyPrefab;

    [Tooltip("What does this interaction do?")]
    public INTERACTIONS typeOfInteraction;

    [Tooltip("What condition(s) needs to be satisfied first in order for this interaction to be usable?")]
    public List<INTERACTION_CONDITIONS> conditions;

    [Tooltip("This is the message that will be displayed to the player.")]

    public string display;

    [Tooltip("Location to which object should be Created or Moved.")]
    public Vector3 locationOfInterest;

    [Tooltip("Object to be Created, Destroyed, or Moved.")]
    public GameObject objectOfInterest;

    [Tooltip("String ID for the item that will be Given or Taken.")]
    public string itemOfInterest;

    [Tooltip("How many items should be Given or Taken.")]
    public int qtyOfItem;

    //Interaction Specific Objects
    private Grill grillObjectReference;


    public enum INTERACTIONS
    {
        giveItem,
        takeItem,
        moveObject,
        createObject,
        deleteObject,
        grill,
        freezer,
        register,
        npc,
        window,
        door,
        toilet,
        food_assembly,
        beverage_machine

    }

    public enum INTERACTION_CONDITIONS
    {
        itemExistsInInventory,
        itemInSelectedInventorySlot,
        globalObjectiveAcheived,
        useGrillConditions
    }

    #region Interaction Types
    private void GrillInteraction()
    {
        //Place a patty on the grill
        if (grillObjectReference.HasCapacity() && (playerInventoryReference.GetSelectedItem() != null) && (playerInventoryReference.GetSelectedItem().id == "patties"))
        {
            playerInventoryReference.Remove("patties", 1, playerInventoryReference.slotSelected);
            grillObjectReference.AddPatty();
            return;
        }

        //Take a patty from the grill
        if ((grillObjectReference.HasDonePatties() == true) && (playerInventoryReference.GetSelectedItem() == null)){
            playerInventoryReference.Add("patties", 1, playerInventoryReference.slotSelected);
            grillObjectReference.RemovePatty();
            return;
        }

    }

    private void takeItemInteraction()
    {
        if (conditions.Contains(INTERACTION_CONDITIONS.itemInSelectedInventorySlot))
        {
            playerInventoryReference.Remove(itemOfInterest, qtyOfItem, playerInventoryReference.slotSelected);
        }
        else
        {
            playerInventoryReference.Remove(itemOfInterest, qtyOfItem);
        }
    }

    #endregion

    #region Interaction Usage
    public void ExecuteInteraction()
    {
        switch (this.typeOfInteraction)
        {
            case INTERACTIONS.grill:
                GrillInteraction();
                break;

            case INTERACTIONS.giveItem:
                playerInventoryReference.Add(itemOfInterest, qtyOfItem);
                break;

            case INTERACTIONS.takeItem:
                takeItemInteraction();
                break;

            case INTERACTIONS.deleteObject:
                break;

            case INTERACTIONS.createObject:
                break;

            case INTERACTIONS.moveObject:
                break;
        }
    }

    //Check if interaction is valid to use
    public bool Valid()
    {
        //check general interaction conditions (belonging to the interaction object)
        if (conditions.Count == 0) { return true; }
        for (int i = 0; i < conditions.Count; i++)
        {
            switch (conditions[i])
            {

                case (INTERACTION_CONDITIONS.itemInSelectedInventorySlot):
                    if ((playerInventoryReference.GetSelectedItem() == null) || (playerInventoryReference.GetSelectedItem().id != itemOfInterest))
                    {
                        return false;
                    }
                    break;

                case (INTERACTION_CONDITIONS.useGrillConditions):
                    if (typeOfInteraction == INTERACTIONS.grill)
                    {
                        //optional condition 1: The player has a patty and there is a free spot on the grill.
                        if ((playerInventoryReference.GetSelectedItem() != null) && (playerInventoryReference.GetSelectedItem().id == "patties") && grillObjectReference.HasCapacity())
                        {
                            return true;
                        }

                        //optional condition 2: The player has a free slot selected and there is a done patty on the grill
                        if ((playerInventoryReference.GetSelectedItem() == null) && (grillObjectReference.HasDonePatties()))
                        {
                            return true;
                        }
                        return false;
                    }
                    break;

                case (INTERACTION_CONDITIONS.itemExistsInInventory):
                    if (playerInventoryReference.ItemPresent(itemOfInterest) == false)
                    {
                        return false;
                    }
                    break;
            }
        }

        return true;
    }

    #endregion

    #region Interaction Error checking
    //Error check Inspector input for this interaction.
    public void VerifyInteractions()
    {
        switch (this.typeOfInteraction)
        {
            case INTERACTIONS.giveItem:
                if (itemOfInterest == ""){ Debug.LogError("Interaction 'Give Item' not configured properly: no 'item of interest' string specified.");}
                if (qtyOfItem <= 0) { Debug.LogError("Interaction 'Give Item' not configured properly: qty of 0 set."); }
                break;

            case INTERACTIONS.takeItem:
                if (itemOfInterest == "") { Debug.LogError("Interaction 'Take Item' not configured: no item specified."); }
                if (qtyOfItem <= 0) { Debug.LogError("Interaction 'Take Item' not configured: qty of 0 set."); }
                break;

            case INTERACTIONS.deleteObject:
                if (objectOfInterest == null) { Debug.LogError("Interaction 'Delete Object' not configured: object to delete set to null."); }
                break;

            case INTERACTIONS.createObject:
                if (objectOfInterest == null) { Debug.LogError("Interaction 'Create Object' not configured: object to create set to null."); }
                break;

            case INTERACTIONS.moveObject:
                if (objectOfInterest == null) { Debug.LogError("Interaction 'Move Object' not configured: object to move set to null."); }
                break;

            case INTERACTIONS.grill:
                if (conditions.Contains(INTERACTION_CONDITIONS.useGrillConditions) == false)
                {Debug.LogError("Interaction 'Grill' not configured properly. Please add 'useGrillConditions under the Conditions section of the inspector");}
                break;
        }

        if (playerInventoryReference == null) {Debug.LogError("Interaction not configured properly. You forgot to add the player's Inventory to the inspector of this object.");}

    }

    #endregion

    private void InitializeEntity()
    {
        switch (typeOfInteraction)
        {
            case INTERACTIONS.grill:
                grillObjectReference = new Grill(15, pattyPrefab);
            break;
        }

    }

    public void Awake()
    {
        VerifyInteractions();   // ensure the interaction was defined properly in the inspector
        InitializeEntity();     //(if applicable) create the entity specific to this interaction

    }

    public void Update()
    {
        switch (typeOfInteraction)
        {
            case (INTERACTIONS.grill):
                grillObjectReference.CookPatties();
                break;
        }
    }

}