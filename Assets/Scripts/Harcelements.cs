using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Harcelements : MonoBehaviour
{
    [Header("Récupération des components")]
    public List<SpriteRenderer> sprites;

    public List<Transform> commentsPos;
    public List<Vector3> initialCommentsPos;

    public List<TextMeshPro> text_GRIS;

    public List<TextMeshPro> text_BLANC;
    public List<RectTransform> text_BLANC_Pos;

    List<int> numbersToChooseFrom;

    [Header("Talking Client _ Animation Curves")]
    public AnimationCurve opacityCurve;
    public AnimationCurve positionsCurve;
    public AnimationCurve textLagCurve;

    public float currentTalkingTime;
    public float maxTalkingTime;

    public float percentOpacity;
    public float percentPositions;
    public float percentTextLag;

    public float opacityMax;
    public float yMaxPos;
    public float posLag;

    public float delayBeforeActivation;
    public float currentDelay;

    public int commentIndex;
    public int nextIndex;

    public bool isDelayOver;
    public bool commentsActivated;
    public bool isCommentSelected;


    [Header("Runaway_ Animation Curve")]
    public AnimationCurve runawayCurve;

    public float currentRunTime;
    public float maxRunTime;
    public float percentRun;

    public Color32 box_Color;
    public Color32 textGris_Color;
    public Color32 textBlanc_Color;

    public float fontShift;
    public float currentFont;

    public bool isCurrentValuesGet;
    public bool isDeactivationNeeded;



    private void Awake()
    {
        for (int i = 0; i < commentsPos.Count; i++)
        {
            initialCommentsPos.Add(commentsPos[i].transform.localPosition);
        }
    }

    private void Update()
    {
        if (commentsActivated)
        {
            if (isCommentSelected && !isDeactivationNeeded)
            {
                if (isDelayOver)
                {
                    if (!isDeactivationNeeded)
                        TalkingCurve();
                }
                else
                {
                    if (currentDelay < delayBeforeActivation)
                    {
                        currentDelay += Time.deltaTime;
                    }
                    else
                    {
                        isDelayOver = true;
                    }
                }
            }
            else
            {
                if (!isDeactivationNeeded)
                {
                    nextIndex = GetNewIndex();

                    if (nextIndex != commentIndex)
                    {
                        commentIndex = nextIndex;
                        isCommentSelected = true;
                    }
                }
            }

            if (isDeactivationNeeded)
            {
                if (!isCurrentValuesGet)
                {
                    GetValues();
                }
                else
                {
                    RunawayCurve();
                }
            }
        }
        else
        {
            if (isDeactivationNeeded)
            {
                if (!isCurrentValuesGet)
                {
                    GetValues();
                }
                else
                {
                    RunawayCurve();
                }
            }
        }
    }


    /// <summary>
    /// Active les voix
    /// </summary>
    public void ActivateClientVoice()
    {
        commentsActivated = true;
    }

    /// <summary>
    /// Retire les voix
    /// </summary>
    public void Silence()
    {
        isCurrentValuesGet = false;
        isDeactivationNeeded = true;
    }

    /// <summary>
    /// Désactive les voix
    /// </summary>
    public void DeactivateClientVoice()
    {
        commentsActivated = false;
        isDelayOver = true;
        isCommentSelected = false;

        isCurrentValuesGet = false;
        isDeactivationNeeded = true;
    }

    #region Randomized Values
    int GetNewIndex()
    {
        return Random.Range(0, sprites.Count - 1);
    }

    float NewDelay()
    {
        return Random.Range(0, 1.5f);
    }
    #endregion

    /// <summary>
    /// Récupère les valeurs du moment pour le runaway curve (pour les faire disparaitre)
    /// </summary>
    private void GetValues()
    {
        box_Color = sprites[commentIndex].color;
        textGris_Color = text_GRIS[commentIndex].color;
        textBlanc_Color = text_BLANC[commentIndex].color;

        currentFont = text_BLANC[commentIndex].fontSize;

        isCurrentValuesGet = true;
    }

    #region Animation Curves
    private void TalkingCurve()
    {
        if (currentTalkingTime < maxTalkingTime)
        {
            currentTalkingTime += Time.deltaTime;

            currentRunTime = 0;
            isCurrentValuesGet = false;
        }
        else
        {
            isCommentSelected = false;
            currentTalkingTime = 0;

            currentDelay = 0;
            delayBeforeActivation = NewDelay();
            isDelayOver = false;
        }

        percentOpacity = opacityCurve.Evaluate(currentTalkingTime / maxTalkingTime);
        percentPositions = positionsCurve.Evaluate(currentTalkingTime / maxTalkingTime);
        percentTextLag = textLagCurve.Evaluate(currentTalkingTime / maxTalkingTime);

        sprites[commentIndex].color = new Color32((byte)0, (byte)0, (byte)0, (byte)(0 + (opacityMax * percentOpacity)));
        text_BLANC[commentIndex].color = new Color32((byte)255, (byte)255, (byte)255, (byte)(0 + 255 * percentOpacity));
        text_GRIS[commentIndex].color = new Color32((byte)170, (byte)170, (byte)170, (byte)(0 + 255 * percentOpacity));

        commentsPos[commentIndex].transform.localPosition = new Vector3(initialCommentsPos[commentIndex].x, (initialCommentsPos[commentIndex].y + (yMaxPos * percentPositions)), initialCommentsPos[commentIndex].z);

        text_BLANC_Pos[commentIndex].localPosition = new Vector3(text_BLANC_Pos[commentIndex].localPosition.x, (0 + (posLag * percentTextLag)), text_BLANC_Pos[commentIndex].localPosition.z);
    }

    private void RunawayCurve()
    {
        if (currentRunTime < maxRunTime)
        {
            currentRunTime += Time.deltaTime;
        }
        else
        {
            text_BLANC[commentIndex].fontSize = currentFont;

            isCommentSelected = false;
            currentTalkingTime = 0;

            currentDelay = 0;
            delayBeforeActivation = NewDelay();
            isDelayOver = false;



            isDeactivationNeeded = false;

        }

        percentRun = runawayCurve.Evaluate(currentRunTime / maxRunTime);

        sprites[commentIndex].color = new Color32(box_Color.r, box_Color.g, box_Color.b, (byte)(box_Color.a * percentRun));
        text_BLANC[commentIndex].color = new Color32(textBlanc_Color.r, textBlanc_Color.g, textBlanc_Color.b, (byte)(textBlanc_Color.a * percentRun));
        text_GRIS[commentIndex].color = new Color32(textGris_Color.r, textGris_Color.g, textGris_Color.b, (byte)(textGris_Color.a * percentRun));

        text_BLANC[commentIndex].fontSize = currentFont - (fontShift - (fontShift * percentRun));
    }
    #endregion
}
