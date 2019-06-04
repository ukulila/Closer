using System.Collections.Generic;
using UnityEngine;

public class NPCInteractions : MonoBehaviour
{

    public List<DialogueClass> dialogue;

    public List<int> questionIndex;

    public int currentDialogueIndex;





    public void StartAnyDialogueViaIndex(int DialogueIndex)
    {
        dialogue[DialogueIndex].questDialogueSystems.StartDialogue();

        currentDialogueIndex = DialogueIndex;

        Examine_Script.Instance.examineButton.interactable = false;
    }

    public void StartDialogueAbout(int DialogueIndex)
    {
        dialogue[questionIndex[DialogueIndex]].questDialogueSystems.StartDialogue();

        currentDialogueIndex = DialogueIndex;

        Examine_Script.Instance.examineButton.interactable = false;
    }
}
