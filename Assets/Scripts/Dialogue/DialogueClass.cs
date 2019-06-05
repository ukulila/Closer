using System.Collections;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class DialogueClass
{
    public DialogueSystem questDialogueSystems;
    public UnityEvent questEvents;
    public bool questHasBeenAsked;
    public bool isNewQuest;
    public string questQuestion;
}
