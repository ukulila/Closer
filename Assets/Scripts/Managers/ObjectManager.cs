using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{

    public Objet_Interaction currentObjet;

    public static ObjectManager Instance;



    private void Awake()
    {
        Instance = this;
    }

    public void CollectCurrentObject()
    {
        InventorySystem.Instance.AssignToAvailableSlot(currentObjet);
        //currentObjet.gameObject.GetComponent<Image>().enabled = false;
        //currentObjet.gameObject.GetComponent<BoxCollider>().enabled = false;
        currentObjet.objectEvent.Invoke();
        ROOM_Manager.Instance.currentRoom.isInteraction = false;
        ROOM_Manager.Instance.currentRoom.objet = null;


        currentObjet = null;
    }
}
