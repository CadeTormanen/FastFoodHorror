using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRider : MonoBehaviour
{

    public GameObject[] positions;
    private float secondsOnPath;
    public float totalSecondsOnPath;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Splines.Cubic(positions[0].transform.position, positions[1].transform.position, positions[2].transform.position, positions[3].transform.position, (secondsOnPath % totalSecondsOnPath)/totalSecondsOnPath);
        secondsOnPath      += Time.deltaTime;
    }
}
