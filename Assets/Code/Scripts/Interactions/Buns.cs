using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class BunsTop : MonoBehaviour, Interaction
=======
public class BunsBottom : MonoBehaviour, Interaction
>>>>>>> parent of 2cb752b8 (Incorporate 'Cade/Pathing' paths, Create customers, register)
{
    public string interactionText { get; set; }
    [SerializeField]
    private Inventory playerInventory;
    [SerializeField]
    private string itemString;
    [SerializeField]
    private int numberOfItemsToGive;

    public bool Possible()
    {
<<<<<<< HEAD
<<<<<<< HEAD:Assets/Code/Scripts/Interactions/BunsTop.cs
        interactionText = "Top Bun";
=======
<<<<<<< HEAD:Assets/Code/Scripts/Interactions/BunsBottom.cs
        interactionText = "Bottom Bun";
>>>>>>> parent of 2cb752b8 (Incorporate 'Cade/Pathing' paths, Create customers, register)
=======
        interactionText = "Take Hamburger\nBun";
>>>>>>> parent of 2cb752b8 (Incorporate 'Cade/Pathing' paths, Create customers, register):Assets/Code/Scripts/Interactions/Buns.cs
        return true;
    }

    public void ExecuteInteraction()
    {
        playerInventory.Add(this.itemString, this.numberOfItemsToGive);
    }

    public void ValidateInteraction()
    {
        if (playerInventory == null) { Debug.LogError("Player Inventory Was Not Set In The Inspector"); }
    }
}
