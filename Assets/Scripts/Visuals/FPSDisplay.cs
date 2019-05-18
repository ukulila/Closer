using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{

    public TMP_Text text;
    private int CurrentFPS;
    private float fps;
    private string fpsString;

    private void Awake()
    {
        Application.targetFrameRate = 400;
    }

    void Update()
    {
        fps = 1.0f / Time.smoothDeltaTime;
        CurrentFPS = (int)fps;

        text.text = CurrentFPS.ToString();
        //if (fps < 30) GUI.contentColor = Color.yellow;
        //else if (fps < 10) GUI.contentColor = Color.red;
        //else GUI.contentColor = Color.green;
        //GUI.Label(new Rect(50, 50, 10000, 5000), "FPS: " + (int)fps);
    }
}
