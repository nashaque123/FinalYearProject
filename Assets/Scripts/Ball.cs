using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 _linearVelocity = new Vector3();
    private Vector3 _angularVelocity = new Vector3();
    private readonly float kMass = 0.45f;
    private float _radius;

    // Start is called before the first frame update
    private void Start()
    {
        _radius = transform.lossyScale.x * 0.5f;
    }

    public Vector3 LinearVelocity
    {
        get
        {
            return _linearVelocity;
        }

        set
        {
            _linearVelocity = value;
        }
    }

    public Vector3 AngularVelocity
    {
        get
        {
            return _angularVelocity;
        }

        set
        {
            _angularVelocity = value;
        }
    }

    public float Mass
    {
        get
        {
            return kMass;
        }
    }

    public float Radius
    {
        get
        {
            return _radius;
        }
    }
}
