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
    //public Image returnIcon;
    //public Button returnButton;

    [Header("Fade IN or OUT")]
    public bool isCurveNeeded = false;
    public bool onReverse = false;

    [Header("Dialogues state")]
    public bool isEveryDialogueDone;
    public Animator returnAnim;



    public static NPC_Manager Instance;





    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
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

        //Debug.Log("AT SET     currentNPC.questionIndex.Count = " + currentNPC.questionIndex.Count);
        if (currentNPC != null)
        {
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
                questionsAnim[i].ResetTrigger("Gone");
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
    }

    /// <summary>
    /// Lance l'apparition des boutons
    /// </summary>
    private void ActivateDialogueOPTION()
    {
        GameManager.Instance.SwitchModeTo(GameManager.GameMode.Dialogue);

        Examine_Script.Instance.examineButton.interactable = false;

        currentNPC.dialogue[0].questDialogueSystems.actorsIcon[0].gameObject.SetActive(true);

        //Debug.Log("AT ACTIVATION     currentNPC.questionIndex.Count = " + currentNPC.questionIndex.Count);

        for (int i = 0; i < currentNPC.questionIndex.Count; i++)
        {
            questionsAnim[i].SetTrigger("FadeIN");
        }

        onReverse = false;
        isCurveNeeded = true;

        returnAnim.ResetTrigger("Off");
        returnAnim.ResetTrigger("Selected");

        if (GameManager.Instance.currentGameMode != GameManager.GameMode.CinematicMode && GameManager.Instance.currentGameMode != GameManager.GameMode.ClueMode)
            returnAnim.SetTrigger("On");
    }

    private void ReturnToDialogueOPTION()
    {
        GameManager.Instance.SwitchModeTo(GameManager.GameMode.Dialogue);

        currentNPC.dialogue[currentNPC.currentDialogueIndex].questDialogueSystems.gameObject.SetActive(false);

        currentNPC.dialogue[0].questDialogueSystems.actorsIcon[0].gameObject.SetActive(true);

        //Debug.Log("AT RETURN     currentNPC.questionIndex.Count = " + currentNPC.questionIndex.Count);

        for (int i = 0; i < currentNPC.questionIndex.Count; i++)
        {
            questionsAnim[i].ResetTrigger("Gone");
            questionsAnim[i].SetTrigger("FadeIN");
        }

        returnAnim.ResetTrigger("Off");
        returnAnim.ResetTrigger("Selected");

        returnAnim.SetTrigger("On");
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
        returnAnim.ResetTrigger("On");

        //returnButton.interactable = false;
        returnAnim.SetTrigger("Selected");
        //returnAnim.SetTrigger("Off");

        for (int i = 0; i < currentNPC.questionIndex.Count; i++)
        {
            //Debug.Log(i);
            questionsAnim[i].ResetTrigger("FadeIN");
            questionsAnim[i].SetTrigger("Gone");
        }

        if (GameManager.Instance.currentGameMode != GameManager.GameMode.CinematicMode && GameManager.Instance.previousGameMode != GameManager.GameMode.Dialogue && GameManager.Instance.previousGameMode != GameManager.GameMode.ClueMode)
            Examine_Script.Instance.examineButton.interactable = true;

        onReverse = true;
        isCurveNeeded = true;
    }


    /// <summary>
    /// Lance la fonction Dialogue du current NPC
    /// </summary>
    public void TalkToCurrentNPC(int dialogueIndex)
    {
        currentNPC.currentDialogueIndex = currentNPC.questionIndex[dialogueIndex];

        currentNPC.dialogue[currentNPC.currentDialogueIndex].questDialogueSystems.gameObject.SetActive(true);

        StartCoroutine(StartDialogueIn(1.5f, dialogueIndex));
        returnAnim.SetTrigger("Off");



        questionsAnim[dialogueIndex].SetBool("hasBeenAsked", true);

        currentNPC.dialogue[currentNPC.questionIndex[dialogueIndex]].isNewQuest = false;

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

        //Debug.Log("currentDialogueIndex = " + currentNPC.currentDialogueIndex);

        currentNPC.dialogue[currentNPC.currentDialogueIndex].questEvents.Invoke();

        yield return new WaitForSeconds(0.1f);

        if (currentNPC != null)
            SetDialogueOPTION();
    }

    public IEnumerator StartDialogueOPTIONAnimIn(float time)
    {
        yield return new WaitForSeconds(time);

        if (GameManager.Instance.currentGameMode != GameManager.GameMode.CinematicMode && GameManager.Instance.currentGameMode != GameManager.GameMode.ClueMode)
            ActivateDialogueOPTION();
    }

    public IEnumerator ReturnToDialogueOPTIONAnimIn(float time)
    {
        yield return new WaitForSeconds(time);

        if (GameManager.Instance.currentGameMode != GameManager.GameMode.CinematicMode && GameManager.Instance.currentGameMode != GameManager.GameMode.ClueMode)
            ReturnToDialogueOPTION();


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

                //Examine_Script.Instance.examineButton.interactable = true;
                //returnButton.interactable = true;
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

                //Examine_Script.Instance.examineButton.interactable = true;
            }
        }


        percent = blancheIconCurve.Evaluate(currentTime / maxTime);


        blancheIcon.color = new Color(1, 1, 1, (0 + (1 * percent)));
        nameBox.color = new Color(1, 1, 1, (0 + (1 * percent)));
        nameCharacter.color = new Color(1, 1, 1, (0 + (1 * percent)));
        //returnIcon.color = new Color(1, 1, 1, (0 + (1 * percent)));
    }
}