using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, Interaction
{
    [SerializeField] private string itemToPickup;
    [SerializeField] private Inventory playerInventory;


    public string interactionText {  get;  set; }

    public void ExecuteInteraction()
    {
       // playerInventory.Add(itemToPickup, 1);
    }

    public void ValidateInteraction()
    {

    }

    public bool Possible()
    {
        interactionText = "Pickup Broom";
        return false;
    }
}
