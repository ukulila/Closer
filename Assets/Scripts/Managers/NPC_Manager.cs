using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC_Manager : MonoBehaviour
{
    public NPCInteractions currentNPC;

    [Header("Questions References")]
    public List<Animator> questionsAnim;
    public List<TextMeshProUGUI> questionsTexts;
    public List<Question_Ref> questionRef;

    [Header("Dialogue OPTION curve")]
    public AnimationCurve blancheIconCurve;
    public float currentTime;
    public float maxTime;
    public float percent;

    [Header("Variables 4 curve")]
    public Image blancheIcon;
    public TextMeshProUGUI nameCharacter;
    public Image nameBox;
    public Image returnIcon;
    public Button returnButton;

    [Header("Fade IN or OUT")]
    public bool isCurveNeeded = false;
    public bool onReverse = false;

    [Header("Dialogues state")]
    public bool isEveryDialogueDone;




    public static NPC_Manager Instance;





    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if (currentNPC != null)
        {
            /*
            if (currentNPC.dialogue[currentNPC.currentDialogueIndex].questDialogueSystems.isDialogueFinished == true && currentNPC.dialogue[currentNPC.currentDialogueIndex].questDialogueSystems.ending == true
            && currentNPC.dialogue[currentNPC.currentDialogueIndex].questDialogueSystems.dialogueBoxReady == true && currentNPC.dialogue[currentNPC.currentDialogueIndex].questDialogueSystems.dialogueHasStarted == true)
            {
                if (currentNPC.dialogue[currentNPC.currentDialogueIndex].questHasBeenAsked == false)
                {
                    currentNPC.dialogue[currentNPC.currentDialogueIndex].questHasBeenAsked = true;

                    StartCoroutine(StartInvokeIn(1.5f));
                }
                else
                {
                    SetDialogueOPTION();
                }
            }
            */
        }

        if (isCurveNeeded)
        {
            AnimationCurveForBlanche();
        }
    }

    /// <summary>
    /// Set les différents boutons nécéssaires (avant leur activation)
    /// </summary>
    public void SetDialogueOPTION()
    {
        int nbrDialogueAsked = 0;

        for (int i = 0; i < currentNPC.questionIndex.Count; i++)
        {
            if (currentNPC.dialogue[currentNPC.questionIndex[i]].isNewQuest == true)
            {
                questionsAnim[i].SetBool("Discovered", true);

                nbrDialogueAsked -= 1;
            }
            else
            {
                questionsAnim[i].SetBool("Discovered", false);
            }

            if (currentNPC.dialogue[currentNPC.questionIndex[i]].questHasBeenAsked == true)
            {
                questionsAnim[i].SetBool("hasBeenAsked", true);

                nbrDialogueAsked += 1;
            }
            else
            {
                questionsAnim[i].SetBool("hasBeenAsked", false);
            }

            questionsTexts[i].text = currentNPC.dialogue[currentNPC.questionIndex[i]].questQuestion;

            questionsAnim[i].ResetTrigger("FadeIN");
            questionsAnim[i].ResetTrigger("Selected");
            questionsAnim[i].ResetTrigger("FadeOUT");
        }

        if (nbrDialogueAsked == currentNPC.questionIndex.Count)
        {
            isEveryDialogueDone = true;
        }
        else
        {
            isEveryDialogueDone = false;
        }
    }

    /// <summary>
    /// Lance l'apparition des boutons
    /// </summary>
    private void ActivateDialogueOPTION()
    {
        GameManager.Instance.SwitchModeTo(GameManager.GameMode.Dialogue);

        currentNPC.dialogue[0].questDialogueSystems.actorsIcon[0].gameObject.SetActive(true);

        for (int i = 0; i < currentNPC.questionIndex.Count; i++)
        {
            questionsAnim[i].SetTrigger("FadeIN");
        }

        onReverse = false;
        isCurveNeeded = true;
    }

    public void ActivateDialogueOptionIN(float time)
    {
        StartCoroutine(StartDialogueOPTIONAnimIn(time));
    }

    /// <summary>
    /// Lance la disparition des boutons
    /// </summary>
    public void DeactivateDialogueOPTION()
    {
        returnButton.interactable = false;

        for (int i = 0; i < currentNPC.questionIndex.Count; i++)
        {
            Debug.Log(i);
            questionsAnim[i].SetTrigger("Gone");
        }

        onReverse = true;
        isCurveNeeded = true;
    }


    /// <summary>
    /// Lance la fonction Dialogue du current NPC
    /// </summary>
    public void TalkToCurrentNPC(int dialogueIndex)
    {
        StartCoroutine(StartDialogueIn(1.5f, dialogueIndex));
        GameManager.Instance.SwitchModeTo(GameManager.GameMode.Dialogue);
    }


    IEnumerator StartDialogueIn(float time, int index)
    {
        yield return new WaitForSeconds(time);

        currentNPC.StartDialogueAbout(index);
    }

    public IEnumerator StartInvokeIn(float time)
    {
        CellPlacement.Instance.ReactivateCells();

        yield return new WaitForSeconds(time);

        currentNPC.dialogue[currentNPC.currentDialogueIndex].questEvents.Invoke();

        yield return new WaitForSeconds(0.1f);

        SetDialogueOPTION();
    }

    public IEnumerator StartDialogueOPTIONAnimIn(float time)
    {
        yield return new WaitForSeconds(time);

        ActivateDialogueOPTION();

        yield return new WaitForSeconds(1.1f);

        Examine_Script.Instance.examineButton.interactable = true;
    }




    /// <summary>
    /// AnimationCurve de Blanche
    /// </summary>
    private void AnimationCurveForBlanche()
    {
        if (!onReverse)
        {
            if (currentTime < maxTime)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                isCurveNeeded = false;

                Examine_Script.Instance.examineButton.interactable = true;
                returnButton.interactable = true;
            }
        }
        else
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                isCurveNeeded = false;
                currentTime = 0;

                Examine_Script.Instance.examineButton.interactable = true;
            }
        }


        percent = blancheIconCurve.Evaluate(currentTime / maxTime);


        blancheIcon.color = new Color(1, 1, 1, (0 + (1 * percent)));
        nameBox.color = new Color(1, 1, 1, (0 + (1 * percent)));
        nameCharacter.color = new Color(1, 1, 1, (0 + (1 * percent)));
        returnIcon.color = new Color(1, 1, 1, (0 + (1 * percent)));
    }
}