using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    public int hp;
    private bool sweepTimeout;
    private float sweepTimeoutLength;
    private float timeSinceLastSweep;

    public void GetSwept() {
        if (sweepTimeout == false){
            Debug.Log("getting swept for real!");
            this.hp -= 50;
            timeSinceLastSweep = 0f;
        } 
    }
    public bool Alive()    { return (hp > 0); }

    public void Start()
    {
        hp = 100;
        sweepTimeout = false;
        timeSinceLastSweep = 0f;
        sweepTimeoutLength = 2f;

    }

    public void Update()
    {
        if (timeSinceLastSweep < sweepTimeoutLength)
        {
            timeSinceLastSweep += Time.deltaTime;
            sweepTimeout = true;
        }
        else
        {
            sweepTimeout = false;
        }
    }

}
