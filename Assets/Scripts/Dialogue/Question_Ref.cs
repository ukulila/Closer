using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_Ref : MonoBehaviour
{
    public int indexReference;
    public Animator boutonAnim;

    public void ReplaceCurrentQuestionTo(int dialogueIndex)
    {
        if (NPC_Manager.Instance.currentNPC.questionIndex.Count == (indexReference + 1))
        {
            NPC_Manager.Instance.currentNPC.questionIndex[indexReference] = dialogueIndex;
        }
        else
        {
            NPC_Manager.Instance.currentNPC.questionIndex.Add(dialogueIndex);
        }
    }

    public void EndOfCelebrity()
    {
        NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.questionIndex[indexReference]].isNewQuest = false;
    }
}
