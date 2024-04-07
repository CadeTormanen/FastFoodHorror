using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fryer : MonoBehaviour, Interaction
{
    private FrenchFry frenchFry;
    [SerializeField]
    private Inventory playerInventory;
    [SerializeField]
    private float timeToFry;

    [SerializeField]
    private GameObject frenchFryPrefab;
    private GameObject frenchFryObject;
    public string interactionText { get; set; }

    private class FrenchFry
    {
        private float timeRequired;
        private float timeElapsed;

        public FrenchFry(float timeRequired)
        {
            this.timeRequired = timeRequired;
        }

        public void Fry()
        {
            if (timeElapsed < timeRequired)
            {
                timeElapsed += Time.deltaTime;
            }
        }
        public bool Done()
        {
            if (timeElapsed >= timeRequired)
            {
                return true;
            }
            return false;
        }
    }

    private bool IsDoneFrying()
    {
       if (frenchFry == null) return false; 
       if (frenchFry.Done() == true) return true;
       return false;
    }

    private bool HasCapacity()
    {
        if (frenchFry == null) return true;
        return false;
    }

    private void RemoveFry()
    {
        if (frenchFry == null) return;
        frenchFryObject.SetActive(false);
        frenchFry = null;
    }

    private void AddFry()
    {
        if (frenchFry != null) return;
        if (frenchFry == null) frenchFry = new FrenchFry(timeToFry);
        frenchFryObject.SetActive(true);
    }

    public void ExecuteInteraction()
    {
        //option 1: player is not holding anything and there is a done fry at the machine: take the fries.
        if ((IsDoneFrying()) && (playerInventory.GetSelectedItem() == null))
        {
            RemoveFry();
            playerInventory.Add("french_fry_done", 1, playerInventory.slotSelected);
        }

        //option 2: player is holding an empty beverage, there is space in the fryer: fry the fries
        if ((HasCapacity()) && (playerInventory.GetSelectedItem() != null) && (playerInventory.GetSelectedItem().id == "french_fry_raw"))
        {
            AddFry();
            playerInventory.Remove("french_fry_raw", 1, playerInventory.slotSelected);
        }

    }

    public void ValidateInteraction()
    {
        if (playerInventory == null) { Debug.LogError("The player inventory was not defined in the inspector for this object."); }
    }

    public bool Possible()
    {
        if ((IsDoneFrying() == true) && (playerInventory.GetSelectedItem() == null))
        {
            interactionText = "Take French Fry";
            return true;
        }

        if ((HasCapacity()) && (playerInventory.GetSelectedItem() != null) && (playerInventory.GetSelectedItem().id == "french_fry_raw"))
        {
            interactionText = "Fry";
            return true;
        }

        return false;
    }

    void Start()
    {
        this.frenchFryObject = Instantiate(frenchFryPrefab, transform.position, Quaternion.identity, transform);
        this.frenchFryObject.transform.Translate(new Vector3(0f, 0f, 0f));
        this.frenchFryObject.SetActive(false);
    }

    void Update()
    {
        if (frenchFry != null) frenchFry.Fry();
    }
}