using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThisIsPolaroid : MonoBehaviour
{
    public Image image;
    public Sprite ClueUnlocked;
    public Sprite ClueLocked;

    public bool isUnlocked;

    public void Update()
    {
        if(isUnlocked)
        {
            image.sprite = ClueUnlocked;
        }
        else
        {
            image.sprite = ClueLocked;
        }
    }
}
