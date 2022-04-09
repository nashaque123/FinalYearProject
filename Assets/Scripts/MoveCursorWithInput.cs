using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursorWithInput : MonoBehaviour
{
    private Vector3 _parentPos;
    private Rect _parentRect;
    public float MovementStepSize;
    private Vector3 _newPos;
    public BooleanScriptableObject GamePlaying;
    public bool IsEnabledWhenGameIsPlaying;
    public bool IsParentBall;

    // Update is called once per frame
    void Update()
    {
        if (GamePlaying.Value.Equals(IsEnabledWhenGameIsPlaying))
        {
            _parentPos = transform.parent.position;
            _parentRect = transform.parent.GetComponent<RectTransform>().rect;
            ProcessHorizontalInput();
            ProcessVerticalInput();
        }
    }

    private void ProcessHorizontalInput()
    {
        if (Input.GetAxis("HorizontalRightAnalogStick") > 0.5f)
        {
            _newPos = new Vector3(transform.position.x + MovementStepSize, transform.position.y, 0);
        }
        else if (Input.GetAxis("HorizontalRightAnalogStick") < -0.5f)
        {
            _newPos = new Vector3(transform.position.x - MovementStepSize, transform.position.y, 0);
        }

        if (IsParentBall)
        {
            if (IsPointWithinBallUI(_newPos))
            {
                transform.position = _newPos;
            }
        }
        else
        {
            if (IsPointWithinSquareUI(_newPos))
            {
                transform.position = _newPos;
            }
        }
    }

    private void ProcessVerticalInput()
    {
        if (Input.GetAxis("VerticalRightAnalogStick") > 0.5)
        {
            _newPos = new Vector3(transform.position.x, transform.position.y + MovementStepSize, 0);
        }
        else if (Input.GetAxis("VerticalRightAnalogStick") < -0.5)
        {
            _newPos = new Vector3(transform.position.x, transform.position.y - MovementStepSize, 0);
        }

        if (IsParentBall)
        {
            if (IsPointWithinBallUI(_newPos))
            {
                transform.position = _newPos;
            }
        }
        else
        {
            if (IsPointWithinSquareUI(_newPos))
            {
                transform.position = _newPos;
            }
        }
    }

    private bool IsPointWithinBallUI(Vector3 point)
    {
        float distance = Vector3.Distance(point, _parentPos);

        //width of image / 2 = radius of ball
        if (distance < (_parentRect.width / 2f) - 1f)
        {
            return true;
        }

        return false;
    }

    private bool IsPointWithinSquareUI(Vector3 point)
    {
        if (point.x > (_parentPos.x - (_parentRect.width / 2f)) && point.x < (_parentPos.x + (_parentRect.width / 2f)) && 
            point.y > (_parentPos.y - (_parentRect.height / 2f)) && point.y < (_parentPos.y + (_parentRect.height / 2f)))
        {
            return true;
        }

        return false;
    }

    public void Reset()
    {
        transform.position = _parentPos;
        _newPos = transform.position;
    }
}
