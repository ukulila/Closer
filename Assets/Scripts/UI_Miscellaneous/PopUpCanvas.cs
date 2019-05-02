using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCanvas : MonoBehaviour
{
    public bool isToggled;
    public Vector3 targetPos;
    public Vector3 offPos;
    public GameObject clue;
    [Range(0.01f, 1)]
    public float speed;

    
    public void Update()
    {
        if (!isToggled)
        {
            clue.transform.localPosition = Vector3.Lerp(transform.localPosition, offPos, speed);            
        }

        if (isToggled)
        {
            clue.transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed);
        }


        if( clue.transform.localPosition == offPos)
        {
            clue.SetActive(false);

        }

    }


    public void PopUp()
    {
        if (!isToggled)
        {
            clue.SetActive(true);
            isToggled = true;
            return;
        }
        else if (isToggled)
        {
            isToggled = false;
            return;

        }


    }




}
