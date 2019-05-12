﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 300;
    }

    void OnGUI()
    {
        float fps = 1.0f / Time.smoothDeltaTime;

        if (fps < 30) GUI.contentColor = Color.yellow;
        else if (fps < 10) GUI.contentColor = Color.red;
        else GUI.contentColor = Color.green;
        GUI.Label(new Rect(0, 0, 500, 250), "FPS: " + (int)fps);
    }
}
