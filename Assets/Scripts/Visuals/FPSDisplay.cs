using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 400;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(50, 50, 1000, 500), "FPS: " + (int)(1.0f / Time.smoothDeltaTime));
    }
}
