using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrowWithInput : MonoBehaviour
{
    private float _newRotationY;
    private readonly float _kMovementStepSize = 1f;

    // Update is called once per frame
    void Update()
    {
        ProcessHorizontalInput();
    }

    private void ProcessHorizontalInput()
    {
        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            _newRotationY = transform.eulerAngles.y + _kMovementStepSize;
        }
        else if (Input.GetAxis("Horizontal") < -0.5f)
        {
            _newRotationY = transform.eulerAngles.y - _kMovementStepSize;
        }

        if (IsValidAngle())
        {
            transform.eulerAngles = new Vector3(0f, _newRotationY, 0f);
        }
    }

    private bool IsValidAngle()
    {
        if (_newRotationY < 90f || _newRotationY > 270f)
        {
            return true;
        }

        return false;
    }
}
