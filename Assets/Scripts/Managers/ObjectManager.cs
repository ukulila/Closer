using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{

    public Objet_Interaction currentObjet;
    public InventorySystem inventorySystem;

    public static ObjectManager Instance;



    private void Awake()
    {
        Instance = this;
    }

    public void CollectCurrentObject()
    {
        inventorySystem.AssignToAvailableSlot(currentObjet);
        currentObjet.gameObject.GetComponent<Image>().enabled = false;
        currentObjet.gameObject.GetComponent<BoxCollider>().enabled = false;

        currentObjet = null;
    }
}
