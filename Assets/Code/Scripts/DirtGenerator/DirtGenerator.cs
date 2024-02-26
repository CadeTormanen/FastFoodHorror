using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtGenerator : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject dirtPrefab;
    [SerializeField] private float width;
    [SerializeField] private float height;
    private int x;
    private int y;

    private GameObject[] dirtPiles;
    public float sweepingRange;
    public int maxPiles;
    private int numPiles;

    public float maxTimeBetweenGenerations;
    public float minTimeBetweenGenerations;
    private float timeBetweenThisGeneration;
    private float timeSinceLastGeneration;
    


    private void CreateDirt()
    {
        if (numPiles >= maxPiles) { return; }

        for(int i=0; i < maxPiles; i++)
        {
            if (dirtPiles[i] == null) { 
                float xPos = Random.Range(0f, width);
                float yPos = Random.Range(0f, height);
                Vector3 placement = new Vector3(xPos, 0, yPos);
                GameObject newDirt = Instantiate(dirtPrefab, transform, false);
                Dirt newDirtComp = newDirt.AddComponent<Dirt>();
                newDirt.transform.Translate(placement);
                dirtPiles[i] = newDirt;
                return;
            }
        }

       
    }

    private void CheckDirt()
    {
        for (int i = 0; i < maxPiles; i++)
        {
            if (dirtPiles[i] != null)
            {
                //check if dirt is being swept
                if ((Vector3.Distance(dirtPiles[i].transform.position, playerObject.transform.position) <= sweepingRange) && playerObject.GetComponent<Player>().sweepState == Player.SWEEPSTATES.sweeping)
                {
                    dirtPiles[i].GetComponent<Dirt>().GetSwept();
                }


                //check if dirt is completely swept
                if (dirtPiles[i].GetComponent<Dirt>().Alive() == false)
                {
                    dirtPiles[i] = null;
                    numPiles--;
                }
            }
        }
    }

    public void Update()
    {
        
        timeSinceLastGeneration += Time.deltaTime;
        if (timeSinceLastGeneration >= timeBetweenThisGeneration)
        {
            CreateDirt();
            timeSinceLastGeneration = 0f;
            timeBetweenThisGeneration = Random.Range(minTimeBetweenGenerations, maxTimeBetweenGenerations);
        }
        CheckDirt();
    }

    public void Start()
    {
        numPiles = 0;
        dirtPiles = new GameObject[maxPiles];
        x = (int) transform.position.x;
        y = (int) transform.position.z;
        timeSinceLastGeneration = 0;
        timeBetweenThisGeneration = Random.Range(minTimeBetweenGenerations, maxTimeBetweenGenerations);
    }


}
