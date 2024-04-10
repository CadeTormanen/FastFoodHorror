using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splines : MonoBehaviour
{
    public List<GameObject> splines;

    public static Vector3 Quadratic(Vector3 v1, Vector3 v2, Vector3 v3, float r)
    {
        Vector3 v1_v2 = Vector3.Lerp(v1, v2, r);
        Vector3 v2_v3 = Vector3.Lerp(v2, v3, r);

        return Vector3.Lerp(v1_v2, v2_v3, r);

    }

    public static Vector3 Cubic(Vector3 v1, Vector3 v2, Vector3 v3, Vector4 v4, float r)
    {
        return Vector3.Lerp(Quadratic(v1, v2, v3, r), Quadratic(v2, v3, v4, r), r);
    }



}
