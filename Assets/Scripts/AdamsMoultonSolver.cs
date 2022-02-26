using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamsMoultonSolver : MonoBehaviour
{
    private Vector3 _velocity = new Vector3(0.15f, 0.9f, 1.1f);
    private float _gravity = -0.1635f; // 9.81 / 60fps
    private float _posX, _posY, _posZ;
    private float _mass = 0.450f;

    // Start is called before the first frame update
    void Start()
    {
        _posX = transform.position.x;
        _posY = transform.position.y;
        _posZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        _velocity = 0.5f * new Vector3(_velocity.x + _velocity.x + 0, _velocity.y + _velocity.y + _gravity, _velocity.z + _velocity.z + 0);
        Debug.Log(_velocity);
        _posX = transform.position.x + _velocity.x;
        _posY = transform.position.y + _velocity.y;
        _posZ = transform.position.z + _velocity.z;
        transform.position = new Vector3(_posX, _posY, _posZ);
    }

    public float Mass
    {
        get
        {
            return _mass;
        }
    }
}
