using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomInteraction : MonoBehaviour
{
    [Header("Room Description")]
    public string roomName = "Enter a room name please";
    public string roomDescription = "Enter a description please";

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descritpionText;
    public TextMeshProUGUI nothingText;
    public SpriteRenderer backgroundSprite;
    public Animator examineAnim;

    [Header("Opportunities")]
    public bool isDialogue;
    public bool isInteraction;

    public NPCInteractions npc;
    public Objet_Interaction objet;

    public Button talkTo;
    public Button interactWith;

    [Header("Clients")]
    public bool isThereClients = false;
    public List<GameObject> clients;

    [Header("Animators")]
    public List<Animator> uiAnimators;


    private void Awake()
    {
        if (objet != null)
            isInteraction = true;

        if (npc != null)
            isDialogue = true;
    }

    /// <summary>
    /// Lance l'apparition de l'UI selon les actions disponibles
    /// </summary>
    public void InteractionAppears()
    {
        uiAnimators[5].ResetTrigger("Enabled");
        uiAnimators[6].ResetTrigger("Enabled");

        uiAnimators[5].ResetTrigger("Disabled");
        uiAnimators[6].ResetTrigger("Disabled");

        uiAnimators[6].ResetTrigger("Selected");

        uiAnimators[5].SetTrigger("Enabled");
        uiAnimators[6].SetTrigger("Disabled");


        Objectif_Scr.Instance.Disappearance();
        ProgressionBar.Instance.Disparition();

        if (UI_Manager.Instance.contextuelleGO[0].activeInHierarchy)
        {
            uiAnimators[2].ResetTrigger("Disabled");
            uiAnimators[3].ResetTrigger("Disabled");
            uiAnimators[4].ResetTrigger("Enabled");

            uiAnimators[2].ResetTrigger("Enabled");
            uiAnimators[3].ResetTrigger("Enabled");
            uiAnimators[4].ResetTrigger("Disabled");

            uiAnimators[2].SetTrigger("Enabled");
            uiAnimators[3].SetTrigger("Enabled");
            

            if (isDialogue)
            {
                talkTo.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Talk to " + npc.gameObject.name;
                uiAnimators[0].ResetTrigger("Enabled");
                uiAnimators[0].ResetTrigger("Disabled");
                uiAnimators[0].SetTrigger("Enabled");
            }

            if (isInteraction)
            {
                interactWith.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Interact with " + objet.objectName;
                uiAnimators[1].ResetTrigger("Enabled");
                uiAnimators[1].ResetTrigger("Disabled");
                uiAnimators[1].SetTrigger("Enabled");
            }

            if (!isInteraction && !isDialogue)
            {
                uiAnimators[4].ResetTrigger("Disabled");
                uiAnimators[4].ResetTrigger("Enabled");
                uiAnimators[4].SetTrigger("Enabled");
            }
        }
    }

    /// <summary>
    /// Assure la disparition de l'UI
    /// </summary>
    public void DisableUI()
    {
        Objectif_Scr.Instance.Appearance();
        ProgressionBar.Instance.Apparition();

        if (UI_Manager.Instance.contextuelleGO[0].activeInHierarchy)
        {
            talkTo.interactable = false;
            uiAnimators[0].ResetTrigger("Enabled");
            uiAnimators[0].SetTrigger("Disabled");

            interactWith.interactable = false;
            uiAnimators[1].ResetTrigger("Enabled");
            uiAnimators[1].SetTrigger("Disabled");

            uiAnimators[2].ResetTrigger("Enabled");
            uiAnimators[2].SetTrigger("Disabled");

            uiAnimators[3].ResetTrigger("Enabled");
            uiAnimators[3].SetTrigger("Disabled");

            if (!isInteraction && !isDialogue)
            {
                uiAnimators[4].ResetTrigger("Enabled");
                uiAnimators[4].SetTrigger("Disabled");
            }
        }

        uiAnimators[5].ResetTrigger("Enabled");
        uiAnimators[5].SetTrigger("Disabled");

        Debug.Log("Disable everything");

        uiAnimators[6].ResetTrigger("Disabled");
        //uiAnimators[6].ResetTrigger("Enabled");
        //uiAnimators[6].ResetTrigger("Selected");
        uiAnimators[6].SetTrigger("Enabled");
    }


    /// <summary>
    /// Change le nom et la description de la pièce
    /// </summary>
    public void UiTextUpdate()
    {
        if (roomName != null)
            nameText.text = roomName;

        if (roomDescription != null)
            descritpionText.text = roomDescription;
    }





    /// <summary>
    /// Coroutine to add an NPC
    /// </summary>
    /// <param name="npcToAdd"></param>
    /// <returns></returns>
    IEnumerator AddingNpc(NPCInteractions npcToAdd)
    {
        CellPlacement.Instance.ReactivateCells();

        yield return new WaitForSeconds(1.5f);

        npcToAdd.gameObject.SetActive(true);
        isDialogue = true;
        npc = npcToAdd;

        CheckInvestigationOpportunities();
    }

    /// <summary>
    /// Coroutine to remove an NPC
    /// </summary>
    /// <param name="npcToRemove"></param>
    /// <returns></returns>
    IEnumerator RemovingNpc(NPCInteractions npcToRemove)
    {
        CellPlacement.Instance.ReactivateCells();

        npc = null;
        isDialogue = false;

        yield return new WaitForSeconds(1.5f);

        npcToRemove.gameObject.SetActive(false);
    }

    /// <summary>
    /// Coroutine to add an OBJECT
    /// </summary>
    /// <param name="objectToAdd"></param>
    /// <returns></returns>
    IEnumerator AddingAnObject(Objet_Interaction objectToAdd)
    {
        CellPlacement.Instance.ReactivateCells();

        yield return new WaitForSeconds(1.8f);

        objectToAdd.gameObject.SetActive(true);
        isInteraction = true;
        objet = objectToAdd;

        CheckInvestigationOpportunities();
    }


    IEnumerator ClientApparition()
    {
        CellPlacement.Instance.ReactivateCells();

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < clients.Count; i++)
        {
            clients[i].SetActive(true);
        }
    }

    IEnumerator ClientDisparition()
    {
        CellPlacement.Instance.ReactivateCells();

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < clients.Count; i++)
        {
            clients[i].SetActive(false);
        }
    }


    public void ClientsCome()
    {
        isThereClients = true;
        StartCoroutine(ClientApparition());
    }

    public void NoClients()
    {
        isThereClients = false;
        StartCoroutine(ClientDisparition());
    }

    /// <summary>
    /// Ajouter un NPC
    /// </summary>
    /// <param name="npcToAdd"></param>
    public void AddNpc(NPCInteractions npcToAdd)
    {
        StartCoroutine(AddingNpc(npcToAdd));
    }

    /// <summary>
    /// Enlever un NPC
    /// </summary>
    /// <param name="npcToRemove"></param>
    public void RemoveNpc(NPCInteractions npcToRemove)
    {
        StartCoroutine(RemovingNpc(npcToRemove));
    }

    /// <summary>
    /// Ajouter un objet
    /// </summary>
    /// <param name="objectToAdd"></param>
    public void AddAnObject(Objet_Interaction objectToAdd)
    {
        StartCoroutine(AddingAnObject(objectToAdd));
    }

    /// <summary>
    /// Check les valeurs de la Room Interaction
    /// </summary>
    public void CheckInvestigationOpportunities()
    {
        if (ROOM_Manager.Instance.currentRoom.isInteraction == true)
        {
            ObjectManager.Instance.currentObjet = ROOM_Manager.Instance.currentRoom.objet;
        }
        else
        {
            ObjectManager.Instance.currentObjet = null;
        }

        if (ROOM_Manager.Instance.currentRoom.isDialogue == true)
        {
            NPC_Manager.Instance.currentNPC = ROOM_Manager.Instance.currentRoom.npc;
        }
        else
        {
            NPC_Manager.Instance.currentNPC = null;
        }
    }
}
