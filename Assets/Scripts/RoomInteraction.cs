using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInteraction : MonoBehaviour
{
    public bool isDialogue;
    public bool isInteraction;
    public bool isSecondFloor;

    public NPCInteractions npc;

    public Button talkTo;
    public Button interactWith;
    public Button changeFloor;


    public List<Animator> buttonsAnimator;




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

        if (isDialogue)
        {
            buttonsAnimator[0].SetTrigger("Enabled");
        }

        if (isInteraction)
        {
            buttonsAnimator[1].SetTrigger("Enabled");
        }

        if (isSecondFloor)
        {
            buttonsAnimator[2].SetTrigger("Enabled");
        }
    }

    /// <summary>
    /// Assure la disparition de l'UI
    /// </summary>
    public void DisableUI()
    {
        //Debug.Log("Disable UI");
        talkTo.interactable = false;
        buttonsAnimator[0].SetTrigger("Disabled");

        interactWith.interactable = false;
        buttonsAnimator[1].SetTrigger("Disabled");

        changeFloor.interactable = false;
        buttonsAnimator[2].SetTrigger("Disabled");
    }

    /// <summary>
    /// Set les Animators
    /// </summary>
    public void SetAnimators()
    {
        buttonsAnimator.Clear();

        if (talkTo != null)
            buttonsAnimator.Add(talkTo.gameObject.GetComponent<Animator>());
        else
            Debug.LogWarning("This Button is not assigned");

        if (interactWith != null)
            buttonsAnimator.Add(interactWith.gameObject.GetComponent<Animator>());
        else
            Debug.LogWarning("This Button is not assigned");

        if (changeFloor != null)
            buttonsAnimator.Add(changeFloor.gameObject.GetComponent<Animator>());
        else
            Debug.LogWarning("This Button is not assigned");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Set le current NPC du manager à l'entrée du joueur
        if (other.gameObject.name.Contains("Player"))
        {
            NPC_Manager.Instance.currentNPC = npc;
            ROOM_Manager.Instance.currentRoom = this;
        }

        //Set le NPC de la room
        if (other.gameObject.name.Contains("NPC"))
        {
            npc = other.gameObject.GetComponent<NPCInteractions>();
        }
    }
}
