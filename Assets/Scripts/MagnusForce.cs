using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnusForce : MonoBehaviour
{
    private readonly float _kPi = 3.1415f;
    public Air Air;

    public Vector3 CalculateMagnusForce(Vector3 angularVelocity)
    {
        Vector3 vortexStrength = 2f * _kPi * GetComponent<SphereCollider>().radius * GetComponent<SphereCollider>().radius * angularVelocity;
        Vector3 lift = Air.Density * new Vector3(Air.Velocity.x * vortexStrength.x, Air.Velocity.y * vortexStrength.y, Air.Velocity.z * vortexStrength.z);
        Vector3 force = GetComponent<SphereCollider>().radius * _kPi * lift / 2;

        return force;
    }
}
