using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamsMoultonSolver : MonoBehaviour
{
    private Ball _ball;
    private readonly float kGravity = -0.1635f; // 9.81 / 60fps
    private float _posX, _posY, _posZ;
    private MagnusForce magnusForce;
    public BooleanScriptableObject ShotTaken;
    public BooleanScriptableObject BallInMotion;
    public GameObject AimArrow;
    public Transform Cursor;
    private Transform _ballUI;

    // Start is called before the first frame update
    void Start()
    {
        _ball = gameObject.GetComponent<Ball>();
        _posX = transform.position.x;
        _posY = transform.position.y;
        _posZ = transform.position.z;
        magnusForce = gameObject.GetComponent<MagnusForce>();
        _ballUI = Cursor.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShotTaken.Value)
        {
            float angleInRadians = MyMathsFunctions.ConvertDegreesToRadians(AimArrow.transform.eulerAngles.y);
            _ball.LinearVelocity = CalculateBallLinearVelocity(angleInRadians);
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
        Vector3 magnusAcceleration = force / _ball.Mass;
        _ball.LinearVelocity = 0.5f * new Vector3(_ball.LinearVelocity.x + _ball.LinearVelocity.x + magnusAcceleration.x, _ball.LinearVelocity.y + _ball.LinearVelocity.y + kGravity + magnusAcceleration.y, _ball.LinearVelocity.z + _ball.LinearVelocity.z + magnusAcceleration.z);

        _posX = transform.position.x + _ball.LinearVelocity.x;
        _posY = transform.position.y + _ball.LinearVelocity.y;
        _posZ = transform.position.z + _ball.LinearVelocity.z;
        transform.position = new Vector3(_posX, _posY, _posZ);
    }

    private Vector3 CalculateBallLinearVelocity(float angleInRadians)
    {
        float yContactPointOnBall = _ballUI.position.y - Cursor.position.y;
        return new Vector3(Mathf.Sin(angleInRadians), 0.6f + (yContactPointOnBall * 0.01f), Mathf.Cos(angleInRadians) * (1.3f - Mathf.Abs(yContactPointOnBall * 0.01f)));
    }
}
