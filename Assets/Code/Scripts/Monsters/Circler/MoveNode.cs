using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : MonoBehaviour
{
    public MoveNode next;
    public MoveNode prev;
    public MoveNode retreatNode; // node to travel to when retreating
    public MoveNode advanceNode; // node to travel to when advancing

    public Vector3 position { get; private set; }

    void Start()
    {
        this.position = transform.position;
        this.GetComponent<MeshRenderer>().enabled = false;
    }
}
