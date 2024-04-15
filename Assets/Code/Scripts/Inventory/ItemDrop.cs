using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDrop : MonoBehaviour, Interaction
{
    public string interactionText { get; set; }
    public ItemData.Item item;
    private Vector3 initPosition;
    private float BOBBING_HEIGHT;
    private float BOBBING_SPEED;
    private float BOBBING_RANGE;


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
        if (playerInventory.SeekFreeSlot() == -1) return false;
        else interactionText = "Take " + item.id; return true;
    }

    public void Bind(ItemData.Item item)
    {
        this.item = item;
    }

    public void Update()
    {
        transform.position = new Vector3(transform.position.x, (float)(initPosition.y + BOBBING_RANGE * Math.Sin(BOBBING_SPEED * Time.fixedTime)), transform.position.z);
        transform.Rotate(0.55f, 0.55f, 0.55f);
    }

    public void Start()
    {
        BOBBING_HEIGHT = 0.4f;
        BOBBING_SPEED = 3.0f;
        BOBBING_RANGE = 0.25f;
        playerInventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        transform.parent = GameObject.Find("interactions").transform;
        initPosition = new Vector3(transform.position.x, BOBBING_HEIGHT, transform.position.y);
    }

}

