using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaTensor
{
    private Matrix3x3 inertiaTensor;
    float sphereValue;

    public InertiaTensor(float mass, float radius)
    {
        sphereValue = 2f / 5f * mass * radius * radius;

        inertiaTensor = new Matrix3x3(
            new Vector3(sphereValue, 0f, 0f),
            new Vector3(0f, sphereValue, 0f),
            new Vector3(0f, 0f, sphereValue));
    }

    public Matrix3x3 LocalCoordinateSystem
    {
        get
        {
            return inertiaTensor;
        }
    }

    public Matrix3x3 Inverse
    {
        get
        {
            return new Matrix3x3(
            new Vector3(1f / sphereValue, 0f, 0f),
            new Vector3(0f, 1f / sphereValue, 0f),
            new Vector3(0f, 0f, 1f / sphereValue));
        }
    }
}
