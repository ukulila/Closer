using System.Collections.Generic;
using UnityEngine;

public class NPCInteractions : MonoBehaviour
{

    public List<DialogueClass> dialogue;

    public List<int> questionIndex;

    public int currentDialogueIndex;




    private void Update()
    {
        
    }

    public void StartAnyDialogueViaIndex(int DialogueIndex)
    {
        dialogue[DialogueIndex].questDialogueSystems.StartDialogue();

        currentDialogueIndex = DialogueIndex;
    }

    public void StartDialogueAbout(int DialogueIndex)
    {
        dialogue[questionIndex[DialogueIndex]].questDialogueSystems.StartDialogue();

        currentDialogueIndex = DialogueIndex;
    }
}
