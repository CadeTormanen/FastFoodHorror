using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DirtGenerator : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject dirtPrefab;
    [SerializeField] private float width;
    [SerializeField] private float height;
    private int x;
    private int y;

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

        float xPos         = Random.Range(0f, width);
        float yPos         = Random.Range(0f, height);
        Vector3 placement  = new Vector3(xPos, 0, yPos);
        GameObject newDirt = Instantiate(dirtPrefab, transform, false);
        Dirt newDirtComp   = newDirt.AddComponent<Dirt>();
        newDirtComp.hp     = 100;
        newDirtComp.alive  = true;
        newDirt.transform.Translate(placement);
        Debug.Log("created a dirt");
        numPiles++;
    }

    private void CheckDirt()
    {
        foreach (Transform child in this.transform)
        {
            // Dirt is dead
            if (child.GetComponent<Dirt>().alive == false)
            {
                Destroy(child.gameObject);
                Debug.Log("destroyed a dirt");
                numPiles--;
                continue;
            }

            //Dirt is dying
            if (child.GetComponent<Dirt>().hp <= 0) {
                continue; 
            }   


            //Dirt is being swept
            if ((Vector3.Distance(child.transform.position, playerObject.GetComponent<Player>().GetBroomHeadPosition()) <= sweepingRange) && playerObject.GetComponent<Player>().sweepState == Player.SWEEPSTATES.sweeping)
            {
                child.GetComponent<Dirt>().Kill();
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
        x = (int) transform.position.x;
        y = (int) transform.position.z;
        timeSinceLastGeneration = 0;
        timeBetweenThisGeneration = Random.Range(minTimeBetweenGenerations, maxTimeBetweenGenerations);
    }


}
