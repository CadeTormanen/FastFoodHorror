using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclerMonster : MonoBehaviour
{
    public MoveNode startingNode;

    private float timeElapsedSinceMove;
    public float timeBetweenOpportunity;

    private void ExecuteMoveOpportunity()
    {

    }


    public void Start()
    {
        if (startingNode == null) { Debug.LogError("No starting node defined for monster "); }
    }

    public void Update()
    {
        transform.position = startingNode.position;
        if ((timeElapsedSinceMove += Time.deltaTime) > timeBetweenOpportunity){
            ExecuteMoveOpportunity();
        }

        
    }


}
