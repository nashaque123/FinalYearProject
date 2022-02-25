using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    public int FPS;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPS;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.targetFrameRate != FPS)
        {
            Application.targetFrameRate = FPS;
        }
    }
}
