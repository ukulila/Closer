using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInteraction : MonoBehaviour
{
    public bool isDialogue;
    public bool isInteraction;
    public bool isSecondFloor;
    public Button talkTo;
    public Button interactWith;
    public Button changeFloor;

    public bool isThereAnObject;



    public void InteractionAppears()
    {
        if (isDialogue)
            talkTo.interactable = true;

        if (isInteraction)
            interactWith.interactable = true;

        if (isSecondFloor)
            changeFloor.interactable = true;
    }


    public void LookAround()
    {

    }


    public void InteractWith()
    {

    }


    public void DialogueWith()
    {

    }


    public void GoTo()
    {

    }
}
