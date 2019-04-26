using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Objet_Interaction : MonoBehaviour
{
    [Header("Definition de l'objet")]
    public string objectName;
    public string objectDescription;
    public Sprite objectImage;

    [Header("Description Parameters & Co")]
    public Image uiObjectImage;
    public TextMeshProUGUI uiObjectDescritpion;


    public void UpdateUItext()
    {
       
    }
}
