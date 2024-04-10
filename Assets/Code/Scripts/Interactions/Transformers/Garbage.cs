using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour, Interaction
{
    public string interactionText { get; set; }
    [SerializeField]
    private Inventory playerInventory;
    [SerializeField]
    private int numberOfItemsToGive;


    public bool Possible()
    {
        if (this.playerInventory.GetSelectedItem() == null) { return false; }
        string item = this.playerInventory.GetSelectedItem().id;
        if (this.playerInventory.IsKeyItem(item) == false)
        {
            interactionText = "Throw Away";
            return true;
        }

        interactionText = "";
        return false;
    }

    public void ExecuteInteraction()
    {
        string item = this.playerInventory.GetSelectedItem().id;
        playerInventory.Remove(item, 1, this.playerInventory.slotSelected);

    }

    public void ValidateInteraction()
    {
        if (playerInventory == null) { Debug.LogError("Player Inventory Was Not Set In The Inspector"); }
    }
}
