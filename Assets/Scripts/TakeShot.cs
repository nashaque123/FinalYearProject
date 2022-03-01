using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeShot : MonoBehaviour
{
    public Transform Cursor;
    private Transform _ballUI;
    private readonly float _maxRevolutionsPerSecond = 9f;
    private float _spinRatePerPixel;
    private readonly float _kPi = 3.1415f;

    // Start is called before the first frame update
    void Start()
    {
        _ballUI = Cursor.parent;
        _spinRatePerPixel = CalculateSpinRatePerPixel();
        Debug.Log("spin rate " + _spinRatePerPixel);
    }

    // Update is called once per frame
    void Update()
    {
        //when player presses shoots, calclate angular velocity and start movement
    }

    private float CalculateSpinRatePerPixel()
    {
        float maxAngularVelocityMagnitudeOnAxesPerSecond = 2f * _kPi * _maxRevolutionsPerSecond;
        float maxSpinRatePerFrame = maxAngularVelocityMagnitudeOnAxesPerSecond / 60f;
        float spinRatePerPixel = maxSpinRatePerFrame / (_ballUI.GetComponent<RectTransform>().rect.width / 2f);

        return spinRatePerPixel;
    }

    private Vector3 CalculateBallAngularVelocity()
    {
        Vector3 differenceBetweenBallAndCursorOnAxes = Cursor.position - _ballUI.position;
        Vector3 angularVelocity = _spinRatePerPixel * differenceBetweenBallAndCursorOnAxes;

        return angularVelocity;
    }
}
