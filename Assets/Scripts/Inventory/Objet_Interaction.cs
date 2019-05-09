using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class Objet_Interaction : MonoBehaviour
{
    [Header("Definition de l'objet")]
    public string objectName;
    public string objectDescription;
    public Sprite objectImage;
    [Space]
    public UnityEvent objectEvent;
}
