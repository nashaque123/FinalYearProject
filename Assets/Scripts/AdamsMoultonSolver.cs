using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamsMoultonSolver : MonoBehaviour
{
    private Ball _ball;
    private readonly float kGravity = -0.1635f; // 9.81 / 60fps
    private float _posX, _posY, _posZ;
    private MagnusForce magnusForce;
    private readonly float _dragCoefficient = 0.25f;
    public BooleanScriptableObject ShotTaken;
    public BooleanScriptableObject BallInMotion;
    public GameObject AimArrow;
    public Transform Cursor;
    private Transform _ballUI;
    public Air Air;
    private readonly float kPi = 3.1415f;
    public FloatScriptableObject PlaySpeedBuffer;
    private int counter = 1;
    public BooleanScriptableObject GamePlaying;

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

        if (BallInMotion.Value && counter % PlaySpeedBuffer.Value == 0 && GamePlaying.Value)
        {
            Move();
        }

        counter++;
    }

    private void Move()
    {
        Vector3 force = magnusForce.CalculateMagnusForce();
        Vector3 magnusAcceleration = force / _ball.Mass;
        float dragFactor = 0.5f * Air.Density * kPi * _ball.Radius * _ball.Radius * _dragCoefficient;
        Vector3 drag = new Vector3(dragFactor * _ball.LinearVelocity.x * _ball.LinearVelocity.x, dragFactor * _ball.LinearVelocity.y * _ball.LinearVelocity.y, dragFactor * _ball.LinearVelocity.z * _ball.LinearVelocity.z);
        _ball.LinearVelocity = 0.5f * new Vector3(_ball.LinearVelocity.x + _ball.LinearVelocity.x + magnusAcceleration.x + drag.x, _ball.LinearVelocity.y + _ball.LinearVelocity.y + kGravity + magnusAcceleration.y + drag.y, _ball.LinearVelocity.z + _ball.LinearVelocity.z + magnusAcceleration.z + drag.z);

        _posX = transform.position.x + _ball.LinearVelocity.x;
        _posY = transform.position.y + _ball.LinearVelocity.y;
        _posZ = transform.position.z + _ball.LinearVelocity.z;
        transform.position = new Vector3(_posX, _posY, _posZ);
    }

    private Vector3 CalculateBallLinearVelocity(float angleInRadians)
    {
        float yContactPointOnBall = _ballUI.position.y - Cursor.position.y;
        return new Vector3(Mathf.Sin(angleInRadians), 0.8f + (yContactPointOnBall * 0.01f), Mathf.Cos(angleInRadians) * (1.4f - Mathf.Abs(yContactPointOnBall * 0.01f)));
    }
}
