using System.Collections;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    public Objet_Interaction currentObjet;

    public static ObjectManager Instance;




    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Ajout de l'objet dans l'inventaire suivi ou non d'un fade pour sa disparition
    /// </summary>
    public void CollectCurrentObject()
    {
        InventorySystem.Instance.AssignToAvailableSlot(currentObjet);

        if (currentObjet.doesTheObjectDisappearAfterInvoke == true)
        {
            FadeScript.Instance.FadeINandOUT();
            StartCoroutine(DelayBeforeInvoke());
        }
        else
        {
            currentObjet.objectEvent.Invoke();

            currentObjet = null;
        }

        ROOM_Manager.Instance.currentRoom.isInteraction = false;
        ROOM_Manager.Instance.currentRoom.objet = null;
    }


    IEnumerator DelayBeforeInvoke()
    {
        yield return new WaitForSeconds(1.5f);

        currentObjet.objectEvent.Invoke();

        yield return new WaitForSeconds(1.5f);

        currentObjet = null;
    }
}
