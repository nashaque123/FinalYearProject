using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamsMoultonSolver : MonoBehaviour
{
    private Vector3 _velocity;
    private readonly float kGravity = -0.1635f; // 9.81 / 60fps
    private float _posX, _posY, _posZ;
    private readonly float kMass = 0.450f;
    private MagnusForce magnusForce;
    public BooleanScriptableObject ShotTaken;
    public BooleanScriptableObject BallInMotion;
    public GameObject AimArrow;

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
            float angleInRadians = MyMathsFunctions.ConvertDegreesToRadians(AimArrow.transform.eulerAngles.y);
            _velocity = CalculateBallLinearVelocity(angleInRadians);
            magnusForce.CalculateBallAngularVelocity(angleInRadians);
            AimArrow.SetActive(false);
            ShotTaken.Value = false;
            BallInMotion.Value = true;
        }

        if (BallInMotion.Value)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 force = magnusForce.CalculateMagnusForce();
        Vector3 magnusAcceleration = force / kMass;
        _velocity = 0.5f * new Vector3(_velocity.x + _velocity.x + magnusAcceleration.x, _velocity.y + _velocity.y + kGravity + magnusAcceleration.y, _velocity.z + _velocity.z + magnusAcceleration.z);

        _posX = transform.position.x + _velocity.x;
        _posY = transform.position.y + _velocity.y;
        _posZ = transform.position.z + _velocity.z;
        transform.position = new Vector3(_posX, _posY, _posZ);
    }

    private Vector3 CalculateBallLinearVelocity(float angleInRadians)
    {
        return new Vector3(Mathf.Sin(angleInRadians), 0.5f, Mathf.Cos(angleInRadians) * 1.3f);
    }

    public Vector3 Velocity
    {
        get
        {
            return _velocity;
        }

        set
        {
            _velocity = value;
        }
    }
}
