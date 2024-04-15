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
    private GameObject pattyRawPrefab;
    [SerializeField]
    private GameObject pattyDonePrefab;
    [SerializeField]
    private Inventory playerInventory;

    private int pattyCount;
    private Patty[] pattyList;
    private GameObject[] pattyRawObjList;
    private GameObject[] pattyDoneObjList;

    AudioSource[] grillSFX;


    private class Patty
    {
        private float timeRequired;
        private float timeElapsed;
        public AudioSource grillingSFX;
        public bool done;

        public Patty(float timeRequired, AudioSource sfx)
        {
            this.timeRequired = timeRequired;
            this.timeElapsed  = 0;
            this.done         = false;
            this.grillingSFX = sfx;
        }
        public void Cook() { timeElapsed += Time.deltaTime; }
        public bool Done() { return (timeElapsed >= timeRequired); }

        public void PlaySound() { this.grillingSFX.Play(); }
        public void StopSound() { this.grillingSFX.Stop(); }
    }

    private int AddPatty()
    {
        if (this.pattyCount == this.pattyCapacity) { Debug.Log("Failed to add patty - beyond capacity"); return -1; }
        for (int i = 0; i < pattyCapacity; i++)
        {
            if (pattyList[i] == null)
            {
                pattyList[i] = new Patty(pattyCookTime, grillSFX[Mathf.Min(i,grillSFX.Length-1)]);
                pattyList[i].PlaySound();
                this.pattyRawObjList[i].SetActive(true);
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
                pattyList[i].StopSound();
                pattyList[i] = null;
                pattyCount--;
                pattyDoneObjList[i].SetActive(false);
                
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
                    pattyRawObjList[i].SetActive(false);
                    pattyDoneObjList[i].SetActive(true);


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
        grillSFX = GetComponents<AudioSource>();
        this.pattyCount       = 0;
        this.pattyList        = new Patty[this.pattyCapacity];
        this.pattyRawObjList  = new GameObject[this.pattyCapacity];
        this.pattyDoneObjList = new GameObject[this.pattyCapacity];
        float offsetOrigin = 0.4f;
        float offsetPatty = 0.3f;

        for (int i = 0; i < pattyCapacity; i++)
        {
            //assign a raw patty prefab to draw while cooking
            this.pattyRawObjList[i] = Instantiate(pattyRawPrefab, transform.position, Quaternion.identity, transform);
            this.pattyRawObjList[i].transform.Translate(new Vector3(0f, 0.20f, i * offsetPatty - offsetOrigin));
            Vector3 prevScale = this.pattyRawObjList[i].transform.localScale;
            prevScale.z = prevScale.z / 2.0f;
            this.pattyRawObjList[i].transform.localScale = prevScale;
            this.pattyRawObjList[i].SetActive(false);
            
            //assign a cooked patty prefab to draw when done cooking.
            this.pattyDoneObjList[i] = Instantiate(pattyDonePrefab, transform.position, Quaternion.identity, transform);
            this.pattyDoneObjList[i].transform.Translate(new Vector3(0f, 0.20f, i * offsetPatty - offsetOrigin));
            this.pattyDoneObjList[i].transform.localScale = prevScale;
            this.pattyDoneObjList[i].SetActive(false);
        }
    }

    void Update()
    {
        this.CookPatties();
    }
}
