using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnusForce : MonoBehaviour
{
    public float _airDensity;
    private Rigidbody _rigidBody;
    private readonly float _pi = 3.1415f;
    public Vector3 _airVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _rigidBody.angularVelocity = new Vector3(-5, 0, 0);
      //  _rigidBody.velocity = new Vector3(3f, 0.1f, 5f);
        //rigidBody.AddForce(100, 10, 100);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vortexStrength = 2f * _pi * GetComponent<SphereCollider>().radius * GetComponent<SphereCollider>().radius * _rigidBody.angularVelocity;
        Vector3 lift = _airDensity * new Vector3(_airVelocity.x * vortexStrength.x, _airVelocity.y * vortexStrength.y, _airVelocity.z * vortexStrength.z);
        Vector3 force = GetComponent<SphereCollider>().radius * _pi * lift / 2;
       // _rigidBody.AddForce(force);
        //Vector3 acceleration = force / rigidBody.mass;
        //rigidBody.velocity += acceleration;
    }
}
