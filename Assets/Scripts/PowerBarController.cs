using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarController : MonoBehaviour
{
    public Slider Slider;
    public FloatScriptableObject ShotPower;

    // Start is called before the first frame update
    void Start()
    {
        ShotPower.Value = 0f;
        Slider.value = Slider.minValue;
    }

    // Update is called once per frame
    void Update()
    {
        Slider.value = ShotPower.Value;
    }
}
