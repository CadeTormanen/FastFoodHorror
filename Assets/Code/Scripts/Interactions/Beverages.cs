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
    [SerializeField]
    private GameObject beveragePrefab;


    private int beverageCapacity;
    private int beverageCount;
    private GameObject[] beverageObjList;
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
            if (beverages[i] == null){ continue; }
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
        for (int i = 0; i < beverageCapacity; i++)
        {
            if (beverages[i] == null) { continue; }
            if (beverages[i].Full() == true)
            {
                this.beverageObjList[i].SetActive(false);
                beverages[i] = null;
                beverageCount--;
                break;
            }
        }
    }

    private void AddBeverage()
    {
        for (int i = 0; i < beverageCapacity; i++)
        {
            if (beverages[i] == null) {
                this.beverageObjList[i].SetActive(true);
                beverages[i] = new beverage(timeToFill);
                beverageCount++;
                break;
            }

        }
    }
    
    public void ExecuteInteraction()
    {
        //option 1: player is not holding anything and there is a full beverage at the machine: take the beverage.
        if ((HasFullBeverages() == true) && (playerInventory.GetSelectedItem() == null))
        {
            RemoveBeverage();
            playerInventory.Add("cup_full", 1, playerInventory.slotSelected);
        }

        //option 2: player is holding an empty beverage, there is space on the beverage machine: fil the beverage
        if ((HasCapacity()) && (playerInventory.GetSelectedItem() != null) && (playerInventory.GetSelectedItem().id == "cup_empty"))
        {
            AddBeverage();
            playerInventory.Remove("cup_empty", 1, playerInventory.slotSelected);
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

        if ((HasCapacity()) && (playerInventory.GetSelectedItem() != null) && (playerInventory.GetSelectedItem().id == "cup_empty"))
        {
            interactionText = "Fill Beverage";
            return true;
        }

        return false;
    }       

    void Start()
    {
        beverageCapacity = 2;
        beverages = new beverage[beverageCapacity];
        this.beverageObjList = new GameObject[this.beverageCapacity];
        float offsetOrigin = 0.25f;
        float offsetBeverage = 0.4f;
        for (int i = 0; i < this.beverageCapacity; i++)
        {
            this.beverageObjList[i] = Instantiate(beveragePrefab, transform.position, Quaternion.identity, transform);
            this.beverageObjList[i].transform.Translate(new Vector3(i * offsetBeverage - offsetOrigin, 0f, -0.30f));
            this.beverageObjList[i].SetActive(false);
        }



    }

    void Update()
    {
        for (int i = 0; i < beverageCapacity; i++)
        {
            if (beverages[i] != null)
            {
                beverages[i].Fill();
            }
            
        }
    }
}