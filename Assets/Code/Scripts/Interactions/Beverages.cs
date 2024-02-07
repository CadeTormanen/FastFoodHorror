using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Beverages : MonoBehaviour, Interaction
{
    private beverage[] beverages;
    [SerializeField]
    private Inventory playerInventory;
    [SerializeField]
    private float timeToFill;
    private int beverageCapacity;
    private int beverageCount;
    
    public string interactionText { get; set; }


    private class beverage
    {
        private bool full;
        private float timeRequired;
        private float timeElapsed;

        public beverage(float timeRequired)
        {
            this.timeRequired = timeRequired;
        }

        public void Fill()
        {
            if (timeElapsed < timeRequired)
            {
                timeElapsed += Time.deltaTime;
            }   
        }
        public bool Full()
        {
            if (timeElapsed >= timeRequired)
            {
                return true;
            }
            return false;
        }
    }

    private bool HasFullBeverages()
    {
        for (int i = 0; i < beverageCapacity; i++)
        {
            if (beverages[i].Full() == true)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasCapacity()
    {
        if (beverageCount < beverageCapacity)
        {
            return true;
        }
        return false;
    }

    private void RemoveBeverage()
    {
        for (int i = 0; i <= beverageCapacity; i++)
        {
            if (beverages[i] != null)
            {
                beverages[i] = null;
            }
        }
    }

    private void AddBeverage()
    {
        for (int i = 0; i <= beverageCapacity; i++)
        {
            if (beverages[i] != null)
            {

                beverageCount++;
            }
        }
    }
    
    
    public void ExecuteInteraction()
    {
        //option 1: player is not holding anything and there is a full beverage at the machine: take the beverage.
        if ((HasFullBeverages() == true) && (playerInventory.GetSelectedItem() == null))
        {
            RemoveBeverage();
            playerInventory.Add("full_beverage", 1, playerInventory.slotSelected);
        }

        //option 2: player is holding an empty beverage, there is space on the beverage machine: fil the beverage
        if ((HasCapacity()) && (playerInventory.GetSelectedItem() != null) && (playerInventory.GetSelectedItem().id == "empty_beverage"))
        {
            AddBeverage();
            playerInventory.Remove("empty_beverage", 1, playerInventory.slotSelected);
        }

    }

    public void ValidateInteraction()
    {
        if (playerInventory == null) { Debug.LogError("The player inventory was not defined in the inspector for this object."); }
    }

    public bool Possible()
    {
        if ((HasFullBeverages() == true) && (playerInventory.GetSelectedItem() == null))
        {
            interactionText = "Take Beverage";
            return true;
        }

        if ((playerInventory.GetSelectedItem() != null) && (playerInventory.GetSelectedItem().id == "empty_beverage"))
        {
            interactionText = "Fill Beverage";
            return true;
        }

        return false;
    }       

    void Start()
    {
        beverages = new beverage[beverageCapacity];
        for (int i = 0; i < beverageCapacity; i++)
        {
            beverages[i] = new beverage(this.timeToFill);
        }

    }

    void Update()
    {
        for (int i = 0; i < beverageCapacity; i++)
        {
            beverages[i].Fill();
        }
    }
}