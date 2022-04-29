using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private Vector3 _normalToSurface;
    public Vector3[] Points = new Vector3[3];

    [Range(0.0f, 1.0f)]
    public float CoefficientOfRestitution;

    // Start is called before the first frame update
    void Start()
    {
        CalculateNormalToSurface();
    }

    private void CalculateNormalToSurface()
    {
        Vector3 vector1 = Points[1] - Points[0];
        Vector3 vector2 = Points[2] - Points[0];
        Vector3 crossProduct = MyMathsFunctions.CalculateCrossProduct(vector1, vector2);

        _normalToSurface = crossProduct / MyMathsFunctions.CalculateVectorMagnitude(crossProduct);
    }

    public Vector3 NormalToSurface
    {
        get
        {
            return _normalToSurface;
        }
    }

    public Vector3 AnyPoint
    {
        get
        {
            return Points[0];
        }
    }
}
