using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomInteraction : MonoBehaviour
{
    [Header("Room Description")]
    public string roomName;
    public string roomDescription;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descritpionText;

    [Header("Opportunities")]
    public bool isDialogue;
    public bool isInteraction;
    public bool isSecondFloor;

    public NPCInteractions npc;

    public Button talkTo;
    public Button interactWith;
    public Button changeFloor;

    

    [Header("Animators")]
    public List<Animator> uiAnimators;




    private void Awake()
    {
        if (npc != null)
            isDialogue = true;
    }

    /// <summary>
    /// Lance l'apparition de l'UI selon les actions disponibles
    /// </summary>
    public void InteractionAppears()
    {

        //Debug.Log("Enable UI");
        uiAnimators[3].SetTrigger("Enabled");
        uiAnimators[4].SetTrigger("Enabled");

        if (isDialogue)
        {
            uiAnimators[0].SetTrigger("Enabled");
        }

        if (isInteraction)
        {
            uiAnimators[1].SetTrigger("Enabled");
        }

        if (isSecondFloor)
        {
            uiAnimators[2].SetTrigger("Enabled");
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
    }

    /// <summary>
    /// Set les Animators
    /// </summary>
    public void SetAnimators()
    {
        uiAnimators.Clear();

        if (talkTo != null)
            uiAnimators.Add(talkTo.gameObject.GetComponent<Animator>());
        else
            Debug.LogWarning("This Button is not assigned");

        if (interactWith != null)
            uiAnimators.Add(interactWith.gameObject.GetComponent<Animator>());
        else
            Debug.LogWarning("This Button is not assigned");

        if (changeFloor != null)
            uiAnimators.Add(changeFloor.gameObject.GetComponent<Animator>());
        else
            Debug.LogWarning("This Button is not assigned");

        if (nameText != null)
            uiAnimators.Add(nameText.gameObject.GetComponent<Animator>());
        else
            Debug.LogWarning("This Button is not assigned");

        if (descritpionText != null)
            uiAnimators.Add(descritpionText.gameObject.GetComponent<Animator>());
        else
            Debug.LogWarning("This Button is not assigned");
    }

    private void UiTextUpdate()
    {
        nameText.text = roomName;
        descritpionText.text = roomDescription;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Set le current NPC du manager à l'entrée du joueur
        if (other.gameObject.name.Contains("Player"))
        {
            NPC_Manager.Instance.currentNPC = npc;
            ROOM_Manager.Instance.currentRoom = this;
            UiTextUpdate();
        }

        //Set le NPC de la room
        if (other.gameObject.name.Contains("NPC"))
        {
            npc = other.gameObject.GetComponent<NPCInteractions>();
        }
    }
}
