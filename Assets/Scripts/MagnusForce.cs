using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnusForce : MonoBehaviour
{
    private readonly float _kPi = 3.1415f;
    public Air Air;
    private readonly float _maxRevolutionsPerSecond = 9f;
    private float _spinRatePerPixel;
    public Transform Cursor;
    private Transform _ballUI;

    // Start is called before the first frame update
    private void Start()
    {
        _ballUI = Cursor.parent;
        _spinRatePerPixel = CalculateSpinRatePerPixel();
    }

    public Vector3 CalculateMagnusForce(float angleInRadians)
    {
        Vector3 angularVelocity = CalculateBallAngularVelocity(angleInRadians);
        Vector3 vortexStrength = 2f * _kPi * gameObject.GetComponent<SphereCollider>().radius * gameObject.GetComponent<SphereCollider>().radius * angularVelocity;
        Vector3 lift = Air.Density * new Vector3(Air.Velocity.x * vortexStrength.x, Air.Velocity.y * vortexStrength.y, Air.Velocity.z * vortexStrength.z);
        Vector3 force = gameObject.GetComponent<SphereCollider>().radius * _kPi * lift / 2;

        return force;
    }

    private float CalculateSpinRatePerPixel()
    {
        float maxAngularVelocityMagnitudeOnAxesPerSecond = 2f * _kPi * _maxRevolutionsPerSecond;
        float maxSpinRatePerFrame = maxAngularVelocityMagnitudeOnAxesPerSecond / 60f;
        float spinRatePerPixel = maxSpinRatePerFrame / (_ballUI.GetComponent<RectTransform>().rect.width / 2f);

        return spinRatePerPixel;
    }

    private Vector3 CalculateBallAngularVelocity(float angleInRadians)
    {
        Vector3 differenceBetweenBallAndCursorOnAxes = _ballUI.position - Cursor.position;
        Vector3 angularVelocity = new Vector3(_spinRatePerPixel * differenceBetweenBallAndCursorOnAxes.x * Mathf.Cos(angleInRadians), _spinRatePerPixel * differenceBetweenBallAndCursorOnAxes.y, _spinRatePerPixel * -differenceBetweenBallAndCursorOnAxes.x * Mathf.Sin(angleInRadians));

        return angularVelocity;
    }
}
