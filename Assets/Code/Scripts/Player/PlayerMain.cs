using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public GameObject inventoryGameObject;
    private Inventory inventoryObject;

    private void FetchInventory()
    {
        if (inventoryGameObject == null) { Debug.LogError("Player could not find inventory! Did you assign the inventory object to player in the inspector?"); }
        inventoryObject = inventoryGameObject.GetComponent<Inventory>();
    }

    private void Inputs()
    {


    }

    void Update()
    {
        Inputs();
    }
    
    void Start()
    {
        FetchInventory();
    }

}
