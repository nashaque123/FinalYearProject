using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeShot : MonoBehaviour
{
    public BooleanScriptableObject ShotTaken;

    // Update is called once per frame
    void Update()
    {
        //when player presses shoot, calculate angular velocity and start movement
        if (Input.GetAxis("FireA") >= 1f)
        {
            ShotTaken.Value = true;
        }
    }
}
