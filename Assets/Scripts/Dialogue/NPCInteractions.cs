﻿using System.Collections.Generic;
using UnityEngine;

public class NPCInteractions : MonoBehaviour
{

    public List<DialogueClass> dialogue;

    public List<int> questionIndex;

    public int currentDialogueIndex;





    public void StartAnyDialogueViaIndex(int index)
    {
        dialogue[index].questDialogueSystems.gameObject.SetActive(true);

        dialogue[index].questDialogueSystems.StartDialogue();

        //NPC_Manager.Instance.currentNPC.currentDialogueIndex = DialogueIndex;

        currentDialogueIndex = index;

        Examine_Script.Instance.examineButton.interactable = false;
    }

    public void StartDialogueAbout(int DialogueIndex)
    {
        dialogue[questionIndex[DialogueIndex]].questDialogueSystems.StartDialogue();

        //NPC_Manager.Instance.currentNPC.currentDialogueIndex = DialogueIndex;

        //currentDialogueIndex = DialogueIndex;

        Examine_Script.Instance.examineButton.interactable = false;
    }
}
