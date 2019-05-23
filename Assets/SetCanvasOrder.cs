using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetCanvasOrder : MonoBehaviour
{
    public Canvas UI_Main;

    public void Backwards()
    {

        UI_Main.sortingOrder = -10;

    }

    public void Front()
    {

        UI_Main.sortingOrder = 10;

    }
}
