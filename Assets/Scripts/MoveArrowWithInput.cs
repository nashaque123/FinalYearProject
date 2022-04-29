using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrowWithInput : MonoBehaviour
{
    private float _newRotationY;
    private readonly float kMovementStepSize = 1f;
    public BooleanScriptableObject GamePlaying;
    public Camera MainCamera;

    // Update is called once per frame
    void Update()
    {
        if (GamePlaying.Value)
        {
            ProcessHorizontalInput();
        }
    }

    //move arrow on user input
    private void ProcessHorizontalInput()
    {
        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            _newRotationY = transform.eulerAngles.y + kMovementStepSize;
        }
        else if (Input.GetAxis("Horizontal") < -0.5f)
        {
            _newRotationY = transform.eulerAngles.y - kMovementStepSize;
        }

        if (IsValidAngle())
        {
            transform.eulerAngles = new Vector3(0f, _newRotationY, 0f);
        }
    }

    //stop player from shooting backwards
    private bool IsValidAngle()
    {
        if (_newRotationY < (90f + MainCamera.transform.eulerAngles.y) % 360f || _newRotationY > (270f + MainCamera.transform.eulerAngles.y) % 360f)
        {
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _newRotationY = MainCamera.transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(0f, MainCamera.transform.eulerAngles.y, 0f);
        gameObject.SetActive(true);
    }
}
