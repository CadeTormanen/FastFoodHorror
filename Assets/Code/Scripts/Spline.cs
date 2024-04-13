using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public List<GameObject> positions;
    public Spline nextSpline;
    public float timeTotalOnPath;

    //Quadratic Interpolation of three points
    public static Vector3 QuadLerp(Vector3 v1, Vector3 v2, Vector3 v3, float r)
    {
        Vector3 v1_v2 = Vector3.Lerp(v1, v2, r);
        Vector3 v2_v3 = Vector3.Lerp(v2, v3, r);

        return Vector3.Lerp(v1_v2, v2_v3, r);

    }

    //Cubic interpolation of four points
    public static Vector3 CubeLerp(Vector3 v1, Vector3 v2, Vector3 v3, Vector4 v4, float r)
    {
        return Vector3.Lerp(QuadLerp(v1, v2, v3, r), QuadLerp(v2, v3, v4, r), r);
    }


    //Cubic interpolation on the current spline
    public Vector3 CubeLerped(float r)
    {
        return CubeLerp(positions[0].transform.position, positions[1].transform.position, positions[2].transform.position, positions[3].transform.position, r);
    }

    public void Start()
    {
        if (positions.Count != 4) Debug.LogError("Incomplete Spline! Each spline needs four position reference objects.");
    }


}
