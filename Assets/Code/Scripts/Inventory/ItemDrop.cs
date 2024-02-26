using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour, Interaction
{
    public string interactionText { get; set; }
    public ItemData.Item item;
    private GameObject model;

    [SerializeField]
    private Inventory playerInventory;

    public void ValidateInteraction()
    {

    }

    public void ExecuteInteraction()
    {
        playerInventory.Add(this.item.id, 1);
        Destroy(gameObject);
    }

    public bool Possible()
    {
        interactionText = "Take " + item.id;
        return true;
    }

    public void Bind(ItemData.Item item)
    {
        this.item = item;
    }

    public void Start()
    {
        playerInventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        transform.parent = GameObject.Find("interactions").transform;
    }

}

