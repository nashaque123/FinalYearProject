using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMathsFunctions
{
    public static float ConvertDegreesToRadians(float degrees)
    {
        return degrees * 3.1415f / 180f;
    }

    public static float CalculateDotProduct(Vector3 v1, Vector3 v2)
    {
        return (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);
    }

    public static float CalculateVectorMagnitude(Vector3 vector)
    {
        float magnitudeSquared = (vector.x * vector.x) + (vector.y * vector.y) + (vector.z * vector.z);
        return Mathf.Sqrt(magnitudeSquared);
    }

    public static float CalculateCosAngleInRadiansBetweenVectors(Vector3 v1, Vector3 v2)
    {
        float dotProduct = CalculateDotProduct(v1, v2);
        float v1Magnitude = CalculateVectorMagnitude(v1);
        float v2Magnitude = CalculateVectorMagnitude(v2);
        return dotProduct / (v1Magnitude * v2Magnitude);
    }

    public static float CalculateAngleInRadiansBetweenVectors(Vector3 v1, Vector3 v2)
    {
        float cosAngle = CalculateCosAngleInRadiansBetweenVectors(v1, v2);
        return Mathf.Acos(cosAngle);
    }

    public static Vector3 CalculateCrossProduct(Vector3 v1, Vector3 v2)
    {
        float x = (v1.y * v2.z) - (v1.z * v2.y);
        float y = (-v1.x * v2.z) + (v1.z * v2.x);
        float z = (v1.x * v2.y) - (v1.y * v2.x);

        return new Vector3(x, y, z);
    }
}
