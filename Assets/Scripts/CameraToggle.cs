using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public Camera[] Cameras = new Camera[3];
    private int _cameraIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetCameraEnabled();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 3") || Input.GetKeyDown("y"))
        {
            _cameraIndex++;
            _cameraIndex %= Cameras.Length;
            SetCameraEnabled();
        }
    }

    private void SetCameraEnabled()
    {
        //set true for camera index and false for rest to ensure only 1 camera enabled
        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].enabled = i == _cameraIndex;
        }
    }
}
