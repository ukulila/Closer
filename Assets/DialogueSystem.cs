using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public List<string> lines;
    public enum Actors { Blanche, SousTenancière, Louis, MmeBerleau, Dotty, Jolly, Dolores, Maggie, Esdie, Walter, Ray, Barney, Irina };
    public Actors currentActor;
    public List<Actors> actors;
    public List<AnimationCurve> linesTypingSpeed;

    public List<Image> dialogueBoxes;
    public List<Image> actorsIcon;

    public int currentLine;
    public int maxLines;
    
    public float boxesInBetweenSpace;
    public float boxMaxWidth;
    public float boxMinWidth;
    public float boxMaxHeight;
    public float boxMinHeight;


    public bool isDialogueFinished;
    public bool endOfTheLine;
    public bool isThereAnotherLine;




    public void StartDialogue()
    {

    }

    public void NextLine()
    {

    }


}
