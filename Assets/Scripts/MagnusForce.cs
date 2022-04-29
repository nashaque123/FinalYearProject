using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnusForce : MonoBehaviour
{
    private Ball _ball;
    private readonly float kPi = 3.1415f;
    public Air Air;
    private readonly float kMaxRevolutionsPerSecond = 12f;
    private float _spinRatePerPixel;
    public Transform Cursor;
    private Transform _ballUI;

    // Start is called before the first frame update
    private void Start()
    {
        _ball = gameObject.GetComponent<Ball>();
        _ballUI = Cursor.parent;
        _spinRatePerPixel = CalculateSpinRatePerPixel();
    }

    //calculate overall force using Kutta-Joukowski theorem
    public Vector3 CalculateMagnusForce()
    {
        Vector3 vortexStrength = 2f * kPi * _ball.Radius * _ball.Radius * _ball.AngularVelocity;
        Vector3 lift = Air.Density * new Vector3(Air.Velocity.x * vortexStrength.x, Air.Velocity.y * vortexStrength.y, Air.Velocity.z * vortexStrength.z);
        Vector3 force = _ball.Radius * kPi * lift / 2f;

        return force;
    }

    //calculate how much spin is applied per pixel due to size of image varying
    private float CalculateSpinRatePerPixel()
    {
        float maxAngularVelocityMagnitudeOnAxesPerSecond = 2f * kPi * kMaxRevolutionsPerSecond;
        float maxSpinRatePerFrame = maxAngularVelocityMagnitudeOnAxesPerSecond / 60f;
        float spinRatePerPixel = maxSpinRatePerFrame / (_ballUI.GetComponent<RectTransform>().rect.width / 2f);

        return spinRatePerPixel;
    }

    //user input calculates initial angular velocity
    public void CalculateBallAngularVelocity(float angleInRadians)
    {
        Vector3 differenceBetweenBallAndCursorOnAxes = _ballUI.position - Cursor.position;
        _ball.AngularVelocity = new Vector3(_spinRatePerPixel * differenceBetweenBallAndCursorOnAxes.x * Mathf.Cos(angleInRadians), _spinRatePerPixel * 0.35f * differenceBetweenBallAndCursorOnAxes.y, _spinRatePerPixel * -differenceBetweenBallAndCursorOnAxes.x * Mathf.Sin(angleInRadians));
    }
}
