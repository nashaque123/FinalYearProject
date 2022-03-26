using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaTensor
{
    private Matrix3x3 inertiaTensor;

    public InertiaTensor(float mass, float radius)
    {
        inertiaTensor = new Matrix3x3(
            new Vector3(2f / 5f * mass * radius * radius, 0f, 0f),
            new Vector3(0f, 2f / 5f * mass * radius * radius, 0f),
            new Vector3(0f, 0f, 2f / 5f * mass * radius * radius));
    }

    public Matrix3x3 LocalCoordinateSystem
    {
        get
        {
            return inertiaTensor;
        }
    }

    /*public Matrix3x3 WorldCoordinateSystem
    {
        get
        {
            return new Matrix3x3(inertiaTensor.Matrix[0] * )
        }
    }*/
}
