using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamsMoultonSolver : MonoBehaviour
{
    private Ball _ball;
    private readonly float kGravity = -0.1635f; // 9.81 / 60fps
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
    public FloatScriptableObject ShotPower;
    public Vector3ScriptableObject BallStartingPosition;

    // Start is called before the first frame update
    void Start()
    {
        _ball = gameObject.GetComponent<Ball>();
        BallStartingPosition.Value = _ball.transform.position;
        _ball.GetComponent<TrailRenderer>().Clear();
        magnusForce = gameObject.GetComponent<MagnusForce>();
        _ballUI = Cursor.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //calculate values based off user input when shot is taken
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

    //update position using adams moulton solver
    private void Move()
    {
        //acceleration applied from magnus force
        Vector3 force = magnusForce.CalculateMagnusForce();
        Vector3 magnusAcceleration = force / _ball.Mass;
        float dragFactor = 0.5f * Air.Density * kPi * _ball.Radius * _ball.Radius * _dragCoefficient;
        Vector3 drag = new Vector3(dragFactor * _ball.LinearVelocity.x * _ball.LinearVelocity.x, dragFactor * _ball.LinearVelocity.y * _ball.LinearVelocity.y, dragFactor * _ball.LinearVelocity.z * _ball.LinearVelocity.z);
        _ball.LinearVelocity = 0.5f * new Vector3(_ball.LinearVelocity.x + _ball.LinearVelocity.x + magnusAcceleration.x + drag.x, _ball.LinearVelocity.y + _ball.LinearVelocity.y + kGravity + magnusAcceleration.y + drag.y, _ball.LinearVelocity.z + _ball.LinearVelocity.z + magnusAcceleration.z + drag.z);

        transform.position += _ball.LinearVelocity;
    }

    //calculate velocity from user input
    private Vector3 CalculateBallLinearVelocity(float angleInRadians)
    {
        float yContactPointOnBall = _ballUI.position.y - Cursor.position.y;
        return new Vector3(Mathf.Sin(angleInRadians), (ShotPower.Value / 2f) + (yContactPointOnBall * 0.01f), Mathf.Cos(angleInRadians) * Mathf.Max(ShotPower.Value - Mathf.Abs(yContactPointOnBall * 0.01f), 0.1f));
    }

    public void Reset()
    {
        transform.position = BallStartingPosition.Value;
        Cursor.GetComponent<MoveCursorWithInput>().Reset();
    }
}
