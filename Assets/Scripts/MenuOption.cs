using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    [Header("Menu Sprites")]
    public Sprite disabledSprite;
    public Sprite selectedSprite;

    [Header("   Component")]
    public Image menuImage;

    [Header("   Parameters")]
    public GameObject panel;
    public bool activated;



    public void OnMenu()
    {
        if (activated)
        {
            activated = false;
            panel.SetActive(false);
            menuImage.sprite = disabledSprite;
        }
        else
        {
            activated = true;
            panel.SetActive(true);
            menuImage.sprite = selectedSprite;
        }
    }
}
