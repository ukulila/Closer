using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Examine_Script : MonoBehaviour
{
    [Header("Examine References")]
    public Image examineImage;
    public Button examineButton;

    [Header("Examine sprites")]
    public Sprite examineIN;
    public Sprite examineOUT;


    public static Examine_Script Instance;



    private void Awake()
    {
        Instance = this;
    }

    public void ExamineOrGetOut()
    {
        if (Camera_UI.Instance.switchToUI == true)
        {
            examineButton.interactable = false;

            Camera_UI.Instance.SwitchToNoUi();
        }
        else
        {
            examineButton.interactable = false;

            ROOM_Manager.Instance.ExamineRoom();
        }
    }
}
