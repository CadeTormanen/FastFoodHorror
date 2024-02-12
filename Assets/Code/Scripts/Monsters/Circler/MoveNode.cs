using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : MonoBehaviour
{
    public MoveNode next;
    public MoveNode prev;
    public Vector3 position { get; private set; }

    void Start()
    {
        this.position = transform.position;
    }
}
