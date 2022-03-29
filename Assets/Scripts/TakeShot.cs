using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeShot : MonoBehaviour
{
    public BooleanScriptableObject ShotTaken;
    public BooleanScriptableObject BallInMotion;
    public BooleanScriptableObject GamePlaying;
    public FloatScriptableObject ShotPower;
    private readonly float _powerIncreaseRate = 0.02f;
    private bool _takenShot = false;

    // Start is called before the first frame update

    private void Start()
    {
        ShotTaken.Value = false;
        BallInMotion.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        //when player presses shoot, calculate angular velocity and start movement
        if (GamePlaying.Value && !_takenShot)
        {
            if (Input.GetAxis("FireA") > 0.5f)
            {
                ShotPower.Value += _powerIncreaseRate;
            }

            if (Input.GetKeyUp("joystick button 0") || Input.GetKeyUp("space"))
            {
                ShotTaken.Value = true;
                _takenShot = true;
            }
        }
    }

    public void Reset()
    {
        _takenShot = false;
        ShotPower.Value = 0f;
    }
}
