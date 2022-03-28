using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeShot : MonoBehaviour
{
    public BooleanScriptableObject ShotTaken;
    public BooleanScriptableObject BallInMotion;
    public BooleanScriptableObject GamePlaying;

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
        if (GamePlaying.Value && (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("space")))
        {
            ShotTaken.Value = true;
        }
    }
}
