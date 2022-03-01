using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamsMoultonSolver : MonoBehaviour
{
    private Vector3 _velocity = new Vector3(0.7f, 0.9f, 1.3f);
    private readonly float _kGravity = -0.1635f; // 9.81 / 60fps
    private float _posX, _posY, _posZ;
    private readonly float _kMass = 0.450f;
    private MagnusForce magnusForce;
    public BooleanScriptableObject ShotTaken;

    // Start is called before the first frame update
    void Start()
    {
        _posX = transform.position.x;
        _posY = transform.position.y;
        _posZ = transform.position.z;
        magnusForce = gameObject.GetComponent<MagnusForce>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShotTaken.Value)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 force = magnusForce.CalculateMagnusForce();
        Vector3 magnusAcceleration = force / _kMass;
        _velocity = 0.5f * new Vector3(_velocity.x + _velocity.x + magnusAcceleration.x, _velocity.y + _velocity.y + _kGravity + magnusAcceleration.y, _velocity.z + _velocity.z + magnusAcceleration.z);

        _posX = transform.position.x + _velocity.x;
        _posY = transform.position.y + _velocity.y;
        _posZ = transform.position.z + _velocity.z;
        transform.position = new Vector3(_posX, _posY, _posZ);
    }
}
