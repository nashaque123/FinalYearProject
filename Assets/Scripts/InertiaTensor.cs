using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaTensor
{
    private readonly Matrix3x3 _inertiaTensor;
    private readonly float _sphereInertiaValue;

    public InertiaTensor(float mass, float radius)
    {
        //hollow sphere inertia for ball
        _sphereInertiaValue = 2f / 3f * mass * radius * radius;

        _inertiaTensor = new Matrix3x3(
            new Vector3(_sphereInertiaValue, 0f, 0f),
            new Vector3(0f, _sphereInertiaValue, 0f),
            new Vector3(0f, 0f, _sphereInertiaValue));
    }

    public Matrix3x3 LocalCoordinateSystem
    {
        get
        {
            return _inertiaTensor;
        }
    }

    public Matrix3x3 Inverse
    {
        get
        {
            return new Matrix3x3(
            new Vector3(1f / _sphereInertiaValue, 0f, 0f),
            new Vector3(0f, 1f / _sphereInertiaValue, 0f),
            new Vector3(0f, 0f, 1f / _sphereInertiaValue));
        }
    }

    public float Inertia
    {
        get
        {
            return _sphereInertiaValue;
        }
    }
}
