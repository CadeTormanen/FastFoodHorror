using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Tooltip("What does this interaction do?")]
    public INTERACTIONS type;

    [Tooltip("What condition(s) needs to be satisfied first in order for this interaction to be usable?")]
    public List<INTERACTION_CONDITIONS> condition;

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

    public enum INTERACTIONS
    {
        giveItem,
        takeItem,
        moveObject,
        createObject,
        deleteObject
    }

    public enum INTERACTION_CONDITIONS
    {
        itemExistsInInventory,
        itemInSelectedInventorySlot,
        globalObjectiveAcheived
    }

    public void ExecuteInteraction()
    {
        switch (this.type)
        {
            case INTERACTIONS.giveItem:
                GameObject.Find("Inventory").GetComponent<Inventory>().Add(itemOfInterest, qtyOfItem);
                break;

            case INTERACTIONS.takeItem:
                GameObject.Find("Inventory").GetComponent<Inventory>().Remove(itemOfInterest, qtyOfItem);
                break;

            case INTERACTIONS.deleteObject:
                break;

            case INTERACTIONS.createObject:
                break;

            case INTERACTIONS.moveObject:
                break;
        }
    }

    //Error check Inspector input for this interaction.
    public void VerifyInteractions()
    {
        switch (this.type)
        {
            case INTERACTIONS.giveItem:
                if (itemOfInterest == ""){ Debug.LogError("Interaction 'Give Item' not configured: no item specified.");}
                if (qtyOfItem <= 0) { Debug.LogError("Interaction 'Give Item' not configured: qty of 0 set."); }
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
        }
    }

    public bool Valid()
    {
        
        return false;
    }

    public void Awake()
    {
        VerifyInteractions();   //ensure all interactions are defined properly.
    }

}