using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSliderInput : MonoBehaviour
{
    public Slider Slider;
    public FloatScriptableObject PlaySpeedBuffer;
    public GameStateMachine GameState;

    // Start is called before the first frame update
    void Start()
    {
        PlaySpeedBuffer.Value = 1f;
        Slider.value = Slider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.Value.Equals(global::GameState.ePaused))
        {
            if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown("k"))
            {
                DecreaseSpeed();
            }

            if (Input.GetKeyDown("joystick button 5") || Input.GetKeyDown("l"))
            {
                IncreaseSpeed();
            }
        }
    }

    private void DecreaseSpeed()
    {
        if (PlaySpeedBuffer.Value < 5f)
        {
            PlaySpeedBuffer.Value+=2;
        }

        Slider.value -= 0.5f;
    }

    private void IncreaseSpeed()
    {
        if (PlaySpeedBuffer.Value > 1f)
        {
            PlaySpeedBuffer.Value-=2;
        }

        Slider.value += 0.5f;
    }
}
