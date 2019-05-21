using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCInteractions : MonoBehaviour
{

    public List<DialogueSystem> communDialogueSystems;
    public List<DialogueSystem> questDialogueSystems;
    public List<UnityEvent> questTriggers;
    public bool onQuest;


    public int dialogueIndex;

    

    /// <summary>
    /// Set le prochain dialogue spécifique
    /// </summary>
    /// <param name="questIndex"></param>
    public void SetDialogueForQuest(int questIndex)
    {
        dialogueIndex = questIndex;
        onQuest = true;
    }

    /// <summary>
    /// Repasse sur les dialogues communs
    /// </summary>
    public void SetDialogueBackToCommun()
    {
        dialogueIndex = 0;
        onQuest = false;
    }

    /// <summary>
    /// Joue les dialogues communs en boucle, et les dialogues de quêtes selon l'index donnée (au cas où un personnage disposerait de plusieurs réponses nécéssitant un dialogue spécifique)
    /// </summary>
    public void StartDialogueAbout()
    {
        if (onQuest)
        {
            questDialogueSystems[dialogueIndex].StartDialogue();

            if (questDialogueSystems[dialogueIndex].isDialogueFinished && !questDialogueSystems[dialogueIndex].ending && questDialogueSystems[dialogueIndex].dialogueBoxReady && questDialogueSystems[dialogueIndex].dialogueHasStarted)
                questTriggers[dialogueIndex].Invoke();
        }
        else
        {
            communDialogueSystems[dialogueIndex].StartDialogue();

            dialogueIndex++;

            if (dialogueIndex > communDialogueSystems.Count - 1)
            {
                dialogueIndex = 0;
            }
        }
    }
}
