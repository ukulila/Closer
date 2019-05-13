using System.Collections.Generic;
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

    [Header("Opportunities")]
    public bool isDialogue;
    public bool isInteraction;
    public bool isSecondFloor;

    public NPCInteractions npc;
    public Objet_Interaction objet;

    public Button talkTo;
    public Button interactWith;
    public Button changeFloor;

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

        //Debug.Log("Enable UI");
        uiAnimators[3].ResetTrigger("Enabled");
        uiAnimators[4].ResetTrigger("Enabled");
        uiAnimators[6].ResetTrigger("Enabled");
        uiAnimators[3].ResetTrigger("Disabled");
        uiAnimators[4].ResetTrigger("Disabled");
        uiAnimators[6].ResetTrigger("Disabled");
        uiAnimators[3].SetTrigger("Enabled");
        uiAnimators[4].SetTrigger("Enabled");
        uiAnimators[6].SetTrigger("Enabled");

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

        if (isSecondFloor)
        {
            uiAnimators[2].ResetTrigger("Enabled");
            uiAnimators[2].ResetTrigger("Disabled");
            uiAnimators[2].SetTrigger("Enabled");
        }

        if (!isSecondFloor && !isInteraction && !isDialogue)
        {
            uiAnimators[5].ResetTrigger("Disabled");
            uiAnimators[5].ResetTrigger("Enabled");
            uiAnimators[5].SetTrigger("Enabled");
        }
    }

    /// <summary>
    /// Assure la disparition de l'UI
    /// </summary>
    public void DisableUI()
    {
        //Debug.Log("Disable UI");

        talkTo.interactable = false;
        uiAnimators[0].SetTrigger("Disabled");

        interactWith.interactable = false;
        uiAnimators[1].SetTrigger("Disabled");

        changeFloor.interactable = false;
        uiAnimators[2].SetTrigger("Disabled");

        uiAnimators[3].SetTrigger("Disabled");

        uiAnimators[4].SetTrigger("Disabled");

        uiAnimators[5].SetTrigger("Disabled");

        uiAnimators[6].SetTrigger("Disabled");
        //uiAnimators[5].ResetTrigger("Disabled");
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


    #region Old Stuff

    //private void OnTriggerEnter(Collider other)
    //{
    //    //Set le NPC de la room
    //    if (other.gameObject.tag == "NPC")
    //    {
    //        npc = other.gameObject.GetComponent<NPCInteractions>();
    //        isDialogue = true;
    //    }

    //    //Set le current NPC du manager à l'entrée du joueur
    //    if (other.gameObject.name.Contains("Player"))
    //    {
    //        ROOM_Manager.Instance.currentRoom = this;

    //        if (npc != null)
    //            NPC_Manager.Instance.currentNPC = npc;

    //        if (objet != null)
    //            ObjectManager.Instance.currentObjet = objet;


    //        UiTextUpdate();
    //    }

    //    if (other.gameObject.tag == "Objet")
    //    {
    //        objet = other.gameObject.GetComponent<Objet_Interaction>();
    //        isInteraction = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.name.Contains("Player"))
    //    {
    //        //ObjectManager.Instance.currentObjet = null;
    //        //ROOM_Manager.Instance.currentRoom = null;
    //    }
    //}

    #endregion
}
