using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    public int hp;
    public bool alive;
    private bool sweepTimeout;
    private float sweepTimeoutLength;
    private float timeSinceLastSweep;
    private MeshRenderer r;
    private Color currentColor;

    public void Kill() {
            this.hp = 0;
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

        if ((hp <= 0) && (r.material.color.a > 0))
        {
            currentColor = r.material.color;
            currentColor.a -= (float)0.1;
            r.material.color = currentColor;
        }

        if (r.material.color.a <= 0) { alive = false; }

    }

    public void Start()
    {
        r = GetComponent<MeshRenderer>();
        sweepTimeout = false;
        timeSinceLastSweep = 0f;
        sweepTimeoutLength = 2f;
    }



}
