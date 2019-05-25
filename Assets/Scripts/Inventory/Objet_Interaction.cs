using UnityEngine;
using UnityEngine.Events;


public class Objet_Interaction : MonoBehaviour
{
    [Header("Definition de l'objet")]
    public string objectName;
    public string objectDescription;
    public Sprite objectImage;
    [Space]
    public UnityEvent objectEvent;

    public bool doesTheObjectDisappearAfterInvoke = true;
}
