using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        currentObjet.gameObject.SetActive(false);
        currentObjet = null;
    }
}
