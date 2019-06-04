using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_Ref : MonoBehaviour
{
    public int indexReference;
    public Animator boutonAnim;

    public void ReplaceCurrentQuestionTo(int dialogueIndex)
    {
        NPC_Manager.Instance.currentNPC.questionIndex[indexReference] = dialogueIndex;
    }

    public void EndOfCelebrity()
    {
        NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.questionIndex[indexReference]].isNewQuest = false;
    }
}
