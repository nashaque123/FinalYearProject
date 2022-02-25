using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnusForce : MonoBehaviour
{
    public float airDensity;
    private Rigidbody rigidBody;
    private readonly float pi = 3.1415f;
    public Vector3 airVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.angularVelocity = new Vector3(-5, 0, 0);
        rigidBody.velocity = new Vector3(3f, 0.1f, 5f);
        //rigidBody.AddForce(100, 10, 100);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vortexStrength = 2f * pi * GetComponent<SphereCollider>().radius * GetComponent<SphereCollider>().radius * rigidBody.angularVelocity;
        Vector3 lift = airDensity * new Vector3(airVelocity.x * vortexStrength.x, airVelocity.y * vortexStrength.y, airVelocity.z * vortexStrength.z);
        Vector3 force = GetComponent<SphereCollider>().radius * pi * lift / 2;
        rigidBody.AddForce(force);
        //Vector3 acceleration = force / rigidBody.mass;
        //rigidBody.velocity += acceleration;
        Debug.Log(rigidBody.velocity);
    }
}
