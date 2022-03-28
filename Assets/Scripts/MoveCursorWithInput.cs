using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursorWithInput : MonoBehaviour
{
    private Vector3 _parentPos;
    private Rect _parentRect;
    private readonly float kMovementStepSize = 1f;
    private Vector3 _newPos;
    public BooleanScriptableObject GamePlaying;

    // Start is called before the first frame update
    void Start()
    {
        _parentPos = transform.parent.position;
        _parentRect = transform.parent.GetComponent<RectTransform>().rect;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlaying.Value)
        {
            ProcessHorizontalInput();
            ProcessVerticalInput();
        }
    }

    private void ProcessHorizontalInput()
    {
        if (Input.GetAxis("HorizontalRightAnalogStick") > 0.5f)
        {
            _newPos = new Vector3(transform.position.x + kMovementStepSize, transform.position.y, 0);
        }
        else if (Input.GetAxis("HorizontalRightAnalogStick") < -0.5f)
        {
            _newPos = new Vector3(transform.position.x - kMovementStepSize, transform.position.y, 0);
        }

        if (IsPointWithinBallUI(_newPos))
        {
            transform.position = _newPos;
        }
    }

    private void ProcessVerticalInput()
    {
        if (Input.GetAxis("VerticalRightAnalogStick") > 0.5)
        {
            _newPos = new Vector3(transform.position.x, transform.position.y + kMovementStepSize, 0);
        }
        else if (Input.GetAxis("VerticalRightAnalogStick") < -0.5)
        {
            _newPos = new Vector3(transform.position.x, transform.position.y - kMovementStepSize, 0);
        }

        if (IsPointWithinBallUI(_newPos))
        {
            transform.position = _newPos;
        }
    }

    private bool IsPointWithinBallUI(Vector3 point)
    {
        float distance = Vector3.Distance(point, _parentPos);

        //width of image / 2 = radius of ball
        if (distance < (_parentRect.width / 2) - 1)
        {
            return true;
        }

        return false;
    }
}
