using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : MonoBehaviour, Interaction
{
    public string interactionText { get; set; }
    [SerializeField]
    private int pattyCapacity;
    [SerializeField]
    private float pattyCookTime;
    [SerializeField]
    private GameObject pattyPrefab;
    [SerializeField]
    private GameObject pattyPrefabDone;
    [SerializeField]
    private Inventory playerInventory;

    private int pattyCount;
    private Patty[] pattyList;
    private GameObject[] pattyObjList;

    private class Patty
    {
        private float timeRequired;
        private float timeElapsed;
        public bool done;

        public Patty(float timeRequired)
        {
            this.timeRequired = timeRequired;
            this.timeElapsed  = 0;
            this.done         = false;
        }
        public void Cook() { timeElapsed += Time.deltaTime; }
        public bool Done() { return (timeElapsed >= timeRequired); }
    }

    private int AddPatty()
    {
        if (this.pattyCount == this.pattyCapacity) { Debug.Log("Failed to add patty - beyond capacity"); return -1; }
        for (int i = 0; i < pattyCapacity; i++)
        {
            if (pattyList[i] == null)
            {
                pattyList[i] = new Patty(pattyCookTime);
                this.pattyObjList[i].SetActive(true);
                pattyCount++;
                return 0;
            }
        }
        return 0;
    }

    private bool RemovePatty()
    {
        if (pattyCount == 0) { return false; }
        for (int i = 0; i < pattyList.Length; i++)
        {
            if ((pattyList[i] != null) && (pattyList[i].done == true))
            {
                pattyList[i] = null;
                pattyCount--;
                this.pattyObjList[i].SetActive(false);
                return true;
            }

        }
        return false;
    }

    public void CookPatties()
    {
        for (int i = 0; i < pattyList.Length; i++)
        {
            Patty patty = pattyList[i];
            if (patty != null)
            {
                patty.Cook();
                if (patty.Done())
                {
                    
                    patty.done = true;
                }
            }
        }
    }

    public bool HasCapacity()
    {
        return (pattyCount != pattyCapacity);
    }

    public bool HasDonePatties()
    {
        for (int i = 0; i < pattyList.Length; i++)
        {
            if ((pattyList[i] != null) && (pattyList[i].done))
            {
                return true;
            }
        }
        return false;
    }

    public void ExecuteInteraction()
    {
        //Place a patty on the grill
        if (this.HasCapacity() && (this.playerInventory.GetSelectedItem() != null) && (this.playerInventory.GetSelectedItem().id == "patty_raw"))
        {
            this.playerInventory.Remove("patty_raw", 1, this.playerInventory.slotSelected);
            this.AddPatty();
            return;
        }

        //Take a patty from the grill
        if ((this.HasDonePatties() == true) && (this.playerInventory.GetSelectedItem() == null))
        {
            this.playerInventory.Add("patty_cooked", 1, this.playerInventory.slotSelected);
            this.RemovePatty();
            return;
        }
    }

    public bool Possible()
    {
        //condition 1: The player has a patty and there is a free spot on the grill.
        if ((this.playerInventory.GetSelectedItem() != null) && (this.playerInventory.GetSelectedItem().id == "patty_raw") && this.HasCapacity())
        {
            interactionText = "Place Raw Patty";
            return true;
        }

        //condition 2: The player has a free slot selected and there is a done patty on the grill
        if ((this.playerInventory.GetSelectedItem() == null) && (this.HasDonePatties()))
        {
            interactionText = "Take Cooked Patty";
            return true;
        }
        interactionText = "";
        return false;
    }


    public void ValidateInteraction()
    {
        if (this.playerInventory == null) { Debug.LogError("Player Inventory Was Not Set In The Inspector"); }
    }

    void Start()
    {
        this.pattyCount   = 0;
        this.pattyList    = new Patty[this.pattyCapacity];
        this.pattyObjList = new GameObject[this.pattyCapacity];
        float offsetOrigin = 0.4f;
        float offsetPatty = 0.3f;
        for (int i = 0; i < pattyCapacity; i++)
        {
            this.pattyObjList[i] = Instantiate(pattyPrefab, transform.position, Quaternion.identity, transform);
            this.pattyObjList[i].transform.Translate(new Vector3(0f, 0.20f, i * offsetPatty - offsetOrigin));
            Vector3 prevScale = this.pattyObjList[i].transform.localScale;
            prevScale.z = prevScale.z / 2.0f;
            this.pattyObjList[i].transform.localScale = prevScale;
            this.pattyObjList[i].SetActive(false);
        }
    }

    void Update()
    {
        this.CookPatties();
    }
}
