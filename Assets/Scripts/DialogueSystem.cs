using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    public TextAsset asset;
    private string textOfAsset;
    private char[] characters;
    private string currentWord;
    private int spaceCount;
    private int lineSpace = 2;
    private bool actorSet = false;
    private bool write = false;


    public List<string> lines;
    public char[] currentLineCharacters;
    public TextMeshProUGUI[] dialogueTexts;
    public GameObject[] dialogueBoxes;
    public GameObject dialogueBoxPrefab;
    public TMP_Vertex vertex;
    public List<Image> actorsIcon;


    public List<AnimationCurve> lineTypingSpeed;
    public float ratioValue;
    public float currentTime;
    public float maxTime;
    public float typingTimeRatio;
    private float typingSpeedRatio = 1;
    private float typingFasterRatio = 1;
    public int currentCharacter;
    public int lastCharacter;

    public enum Actors { Blanche, Mireille, Louis, MmeBerleau, Dotty, Jolly, Dolores, Maggie, Esdie, Walter, Ray, Barney, Irina };
    public Actors currentActor;
    public List<Actors> actors;
    public List<int> activeActorsIndex;



    [Header("Dialogue 'Content'")]
    public int currentLine;
    public int maxLines;

    [Header("Dialogue Box Image Parameters")]
    //Parametres BOX des rectTransform
    public float dialogueBoxSpacing;
    private float boxMaxWidth = 363f;
    private float boxMinWidth = 100f;

    private float boxMinHeight = 80f;
    public float boxHeigthPerLine = 25f;

    private float boxInitPos_X = -445f;
    private float boxInitPos_Y = -150f;

    private float boxInitPos_X2 = -125f;
    private float boxInitPos_Y2 = -150f;


    //Parametres TEXT des rectTransform
    private float textHeightPerLine = 35f;
    private float textMinHeight = 35f;
    public float textWidth = 320;

    //Différents etats du Dialogue
    public bool isDialogueFinished = true;
    public bool endOfTheLine;
    public bool writting;
    public bool isThereAnotherLine;
    public bool ending;
    public bool dialogueHasStarted;


    //Changer la couleur character per character
    public TMP_Text m_TextComponent;
    public TMP_TextInfo textInfo;
    public Color32[] vertexNewColor;


    public List<float> translationCount;
    public RectTransform dialogueGo;
    public bool boxReady = true;
    public Vector2 nextLinePosition;

    public AnimationCurve boxSlidingCurve;
    public float currentSlideTime;
    public float maxSlideTime;
    public float currentSlidePercent;
    public float currentYPos;
    public float nextYPos;

    public AnimationCurve resetDialogueCurve;
    public float currentResetTime;
    public float maxResetTime;

    public float lastY;
    public float percentY;
    public float Ydisplacment;


    public List<float> dialogueBoxWidths;
    public List<float> dialogueBoxHeights;
    public bool dialogueBoxReady = true;

    public AnimationCurve boxAppearingCurve;
    public float currentAppearingTime;
    public float maxAppearingTime = 0.5f;
    public float appearingPercent;
    public float AppearTimeRatio = 1;

    public Color speakingColor;
    public Color listeningColor;

    //Adapter les boxes à leur text
    public Vector2 textSize;
    public float rendWidth;





    private void Awake()
    {

        dialogueHasStarted = false;

        //StartDialogue();


        if (currentLine == maxLines - 1)
        {
            isThereAnotherLine = false;
            ending = true;
        }
        else
        {
            isThereAnotherLine = true;
        }
    }


    public void StartDialogue()
    {
        StartCoroutine(StartDialogueIn(1));
    }


    /// <summary>
    /// Démarer le dialogue après un delai dès l'activation du GameObject
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator StartDialogueIn(float time)
    {
        yield return new WaitForSeconds(time);
        isDialogueFinished = false;
        ActivateActorsIcons();
        dialogueBoxReady = false;
        dialogueHasStarted = true;

        if (currentLine == maxLines - 1)
        {
            isThereAnotherLine = false;
            ending = true;
        }
        else
        {
            isThereAnotherLine = true;
        }
    }


    private void Update()
    {
        if (endOfTheLine && !isDialogueFinished && currentCharacter > 0)
            currentCharacter = 0;

        /*
        if (Input.GetMouseButtonDown(1))
        {
            StartDialogue();
        }
        */


        if (Input.GetMouseButtonDown(0))
        {
            if (endOfTheLine && !isThereAnotherLine)
                isDialogueFinished = true;

            if (endOfTheLine && isThereAnotherLine)
            {
                endOfTheLine = false;
                NextLine();
            }

            if (!endOfTheLine)
                typingTimeRatio = typingFasterRatio;

            if (isDialogueFinished && !ending && dialogueBoxReady && dialogueHasStarted)
                ResetDialogueParameters();
        }

        if (isDialogueFinished && ending)
            EndDialogueAnimation();

        if (!boxReady && !isDialogueFinished)
            SlideDialogueTo();

        if (!dialogueBoxReady && !isDialogueFinished)
            DialogueBoxAppear();

        if (!isDialogueFinished && !endOfTheLine && writting)
            DialogueUpdate();
    }


    #region Dialogue EDITOR SET UP
    public void SetUpTextFile()
    {
        textOfAsset = asset.ToString();

        characters = textOfAsset.ToCharArray();

        if (dialogueGo == null)
        {
            dialogueGo = this.GetComponent<RectTransform>();
        }
    }

    public void SetUpDialogueLines()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (char.IsWhiteSpace(characters[i]))
            {
                spaceCount++;

                if (spaceCount == lineSpace)
                {
                    spaceCount = 0;
                    actorSet = false;
                    write = false;
                }

                if (!actorSet)
                {
                    if (currentWord != null)
                    {
                        if (currentWord == Actors.Barney.ToString().ToUpper())
                        {
                            actors.Add(Actors.Barney);
                            spaceCount = 0;
                            actorSet = true;

                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Blanche.ToString().ToUpper())
                        {
                            actors.Add(Actors.Blanche);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Dolores.ToString().ToUpper())
                        {
                            actors.Add(Actors.Dolores);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Dotty.ToString().ToUpper())
                        {
                            actors.Add(Actors.Dotty);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Esdie.ToString().ToUpper())
                        {
                            actors.Add(Actors.Esdie);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Irina.ToString().ToUpper())
                        {
                            actors.Add(Actors.Irina);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }


                        if (currentWord == Actors.Jolly.ToString().ToUpper())
                        {
                            actors.Add(Actors.Jolly);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Louis.ToString().ToUpper())
                        {
                            actors.Add(Actors.Louis);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Maggie.ToString().ToUpper())
                        {
                            actors.Add(Actors.Maggie);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }


                        if (currentWord == Actors.Mireille.ToString().ToUpper())
                        {
                            actors.Add(Actors.Mireille);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.MmeBerleau.ToString().ToUpper())
                        {
                            actors.Add(Actors.MmeBerleau);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Ray.ToString().ToUpper())
                        {
                            actors.Add(Actors.Ray);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }

                        if (currentWord == Actors.Walter.ToString().ToUpper())
                        {
                            actors.Add(Actors.Walter);
                            spaceCount = 0;
                            actorSet = true;
                            lines.Add("");
                            currentWord = "";
                        }
                    }
                }

                //Debug.Log(currentWord);
                currentWord = "";
            }
            else
            {
                spaceCount = 0;

                if (actorSet)
                    write = true;

                currentWord = currentWord + characters[i];
            }

            //Debug.Log("Lines count is : " + lines.Count);


            if (write)
            {
                lines[lines.Count - 1] = lines[lines.Count - 1] + characters[i];
            }
        }
    }

    public void SetUpDialogueBox()
    {
        maxLines = lines.Count;

        dialogueBoxes = new GameObject[maxLines];
        dialogueTexts = new TextMeshProUGUI[maxLines];
        lineTypingSpeed = new List<AnimationCurve>();

        for (int i = 0; i < maxLines; i++)
        {
            lineTypingSpeed.Add(new AnimationCurve());
            lineTypingSpeed[i].AddKey(0, 0);

            float value = (lines[i].ToCharArray().Length / ratioValue);

            if (value < 1)
                value = 1;

            lineTypingSpeed[i].AddKey(value, lines[i].ToCharArray().Length);

            GameObject currentGO = Instantiate(dialogueBoxPrefab, transform);
            dialogueBoxes.SetValue(currentGO, i);

            dialogueTexts.SetValue(dialogueBoxes[i].GetComponentInChildren<TextMeshProUGUI>(), i);
            dialogueTexts[i].text = lines[i];


            if (actors[i] == Actors.Blanche)
            {
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchorMin = new Vector2(0, 1);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchorMax = new Vector2(0, 1);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().pivot = new Vector2(0, 1);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(boxInitPos_X, boxInitPos_Y);
            }
            else
            {
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchorMin = new Vector2(1, 1);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchorMax = new Vector2(1, 1);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().pivot = new Vector2(1, 1);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(boxInitPos_X2, boxInitPos_Y2);
            }


            if (dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount > 1)
            {
                Debug.Log("i = " + i + " avec " + dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineInfo.Length + " ligne(s)");
                Debug.Log("ie = " + i + " avec " + dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount + " ligne(s)");


                dialogueTexts[i].GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, textMinHeight + ((dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount - 1) * textHeightPerLine));
                dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(boxMaxWidth, boxMinHeight + ((dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount - 1) * boxHeigthPerLine));
            }
            else
            {
                GUIContent content = new GUIContent(dialogueTexts[i].text);
                textSize = GUI.skin.box.CalcSize(content);

                rendWidth = dialogueTexts[i].renderedWidth;

                float widthRatio = textSize.x / rendWidth;


                dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(boxMaxWidth, boxMinHeight);
                dialogueTexts[i].GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, textMinHeight);

            }


        }


        for (int i = 1; i < maxLines; i++)
        {

            float posYdiff = ((dialogueBoxes[i].GetComponent<RectTransform>().anchoredPosition.y - dialogueBoxes[i].GetComponent<RectTransform>().offsetMax.y) +
                (dialogueBoxes[i - 1].GetComponent<RectTransform>().offsetMin.y - dialogueBoxes[i - 1].GetComponent<RectTransform>().anchoredPosition.y)) - dialogueBoxSpacing;

            //Debug.Log("i = " + i + " off :" + dialogueBoxes[i].GetComponent<RectTransform>().position);

            translationCount.Add(posYdiff);

            dialogueBoxes[i].GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(dialogueBoxes[i].GetComponent<RectTransform>().anchoredPosition.x, (dialogueBoxes[i - 1].GetComponent<RectTransform>().anchoredPosition.y + translationCount[i - 1]));
        }

        for (int i = 0; i < maxLines; i++)
        {
            dialogueBoxHeights.Add(dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta.y);
            dialogueBoxWidths.Add(dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta.x);

            dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }
    }

    public void CleanDialogueSetUp()
    {
        textOfAsset = "";
        characters = new char[0];
        actorSet = false;
        write = false;

        for (int i = 0; i < dialogueBoxes.Length; i++)
        {
            if (dialogueBoxes[i] != null)
                DestroyImmediate(dialogueBoxes[i]);
        }

        dialogueBoxes = new GameObject[0];
        dialogueTexts = new TextMeshProUGUI[0];
        lineTypingSpeed.Clear();
        lines.Clear();
        actors.Clear();
        dialogueGo.position = new Vector2(dialogueGo.position.x, dialogueGo.position.y);
        translationCount.Clear();
        activeActorsIndex.Clear();
        currentWord = "";
    }

    public void SetActorsParameters()
    {
        for (int i = 0; i < actors.Count - 1; i++)
        {
            if (actors[i] == Actors.Blanche)
            {
                actorsIcon[(int)Actors.Blanche].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Mireille)
            {
                actorsIcon[(int)Actors.Mireille].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Louis)
            {
                actorsIcon[(int)Actors.Louis].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.MmeBerleau)
            {
                actorsIcon[(int)Actors.MmeBerleau].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Dotty)
            {
                actorsIcon[(int)Actors.Dotty].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Jolly)
            {
                actorsIcon[(int)Actors.Jolly].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Dolores)
            {
                actorsIcon[(int)Actors.Dolores].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Maggie)
            {
                actorsIcon[(int)Actors.Maggie].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Esdie)
            {
                actorsIcon[(int)Actors.Esdie].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Walter)
            {
                actorsIcon[(int)Actors.Walter].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Ray)
            {
                actorsIcon[(int)Actors.Ray].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Barney)
            {
                actorsIcon[(int)Actors.Barney].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Irina)
            {
                actorsIcon[(int)Actors.Irina].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < actorsIcon.Count - 1; i++)
        {
            if (actorsIcon[i].IsActive())
            {
                activeActorsIndex.Add(i);
            }
        }

        DesactivateIcons();
    }

    public void DesactivateIcons()
    {
        for (int i = 0; i < activeActorsIndex.Count; i++)
        {
            actorsIcon[(activeActorsIndex[i])].gameObject.SetActive(false);
        }
    }
    #endregion


    public void ActivateActorsIcons()
    {
        for (int i = 0; i < activeActorsIndex.Count; i++)
        {
            actorsIcon[activeActorsIndex[i]].gameObject.SetActive(true);
        }

        SetDialogueParameters();
    }

    public void SlideDialogueTo()
    {
        if (currentSlideTime < maxSlideTime)
        {
            currentSlideTime += Time.deltaTime;
        }
        else
        {
            dialogueBoxReady = false;
            lastY = dialogueGo.anchoredPosition.y;
            boxReady = true;
        }

        currentSlidePercent = boxSlidingCurve.Evaluate(currentSlideTime);

        dialogueGo.anchoredPosition = new Vector2(dialogueGo.anchoredPosition.x, currentYPos + nextYPos * (currentSlidePercent / maxSlideTime));
    }


    private void DialogueBoxAppear()
    {
        if (currentAppearingTime < maxAppearingTime)
        {
            currentAppearingTime += Time.deltaTime * AppearTimeRatio;
        }
        else
        {
            writting = true;
            dialogueBoxReady = true;
        }

        appearingPercent = boxAppearingCurve.Evaluate(currentAppearingTime);

        dialogueBoxes[currentLine].GetComponent<RectTransform>().sizeDelta = new Vector2(dialogueBoxWidths[currentLine] * (appearingPercent / maxAppearingTime), dialogueBoxHeights[currentLine] * (appearingPercent / maxAppearingTime));
    }


    public void ResetDialogueParameters()
    {
        currentTime = 0;
        currentSlideTime = 0;
        currentCharacter = 0;
        lastCharacter = -1;
        currentLine = 0;
        currentAppearingTime = 0;
        typingTimeRatio = typingSpeedRatio;


        endOfTheLine = false;
        writting = false;
        ending = false;
        boxReady = true;
        dialogueBoxReady = false;
        dialogueHasStarted = false;


        for (int i = 0; i < maxLines; i++)
        {
            dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

        dialogueGo.anchoredPosition = new Vector2(0, 0);

        DesactivateIcons();

        //this.gameObject.SetActive(false);

        /*
                for (int i = 0; i < maxLines - 1; i++)
                {

                    for (int y = 1; y < dialogueTexts[i].textInfo.characterCount; y++)
                    {
                        //Debug.LogWarning("current character = " + currentCharacter);
                        // Get the index of the material used by the current character.
                        int materialIndex = textInfo.characterInfo[y].materialReferenceIndex;
                        //Debug.Log("material Index = " + materialIndex);

                        // Get the vertex colors of the mesh used by this text element (character or sprite).
                        vertexNewColor = textInfo.meshInfo[materialIndex].colors32;
                        // Get the index of the first vertex used by this text element.
                        int vertexIndex = textInfo.characterInfo[y].vertexIndex;

                        // Set all to full alpha
                        vertexNewColor[vertexIndex + 0].a = 0;
                        vertexNewColor[vertexIndex + 1].a = 0;
                        vertexNewColor[vertexIndex + 2].a = 0;
                        vertexNewColor[vertexIndex + 3].a = 0;

                        dialogueTexts[i].UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    }

                }
        */
        //this.gameObject.SetActive(false);
        //dialogueGo.gameObject.SetActive(false);
    }

    public void SetDialogueParameters()
    {
        currentTime = 0;
        currentCharacter = 0;
        lastCharacter = -1;
        typingTimeRatio = typingSpeedRatio;

        maxTime = lineTypingSpeed[currentLine].keys[lineTypingSpeed[currentLine].length - 1].time;

        m_TextComponent = dialogueTexts[currentLine].GetComponent<TMP_Text>();
        textInfo = m_TextComponent.textInfo;

        currentActor = actors[currentLine];


        if ((int)currentActor == activeActorsIndex[0])
        {
            //Debug.Log("SPEAK");
            actorsIcon[activeActorsIndex[0]].color = speakingColor;
            actorsIcon[activeActorsIndex[1]].color = listeningColor;
        }
        else
        {
            //Debug.Log("LISTEN");
            actorsIcon[activeActorsIndex[1]].color = speakingColor;
            actorsIcon[activeActorsIndex[0]].color = listeningColor;
        }

    }


    /// <summary>
    /// Parametres de déplacement des boxes
    /// </summary>
    private void SetSlideParameters()
    {
        nextLinePosition = new Vector2(dialogueGo.anchoredPosition.x, dialogueGo.anchoredPosition.y - translationCount[currentLine - 1]);
        currentYPos = dialogueGo.anchoredPosition.y;
        nextYPos = nextLinePosition.y - currentYPos;

        currentSlideTime = 0;
        currentAppearingTime = 0;
    }


    public void DialogueUpdate()
    {
        if (currentTime < maxTime)
        {
            currentTime += Time.deltaTime * typingTimeRatio;
        }
        else
        {
            endOfTheLine = true;
            writting = false;
            typingTimeRatio = typingSpeedRatio;
        }

        currentCharacter = (int)lineTypingSpeed[currentLine].Evaluate(currentTime);

        if (currentCharacter != lastCharacter)
        {
            //Debug.Log("Last character begin = " + lastCharacter);

            //Debug.LogWarning("current character = " + currentCharacter);
            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
            //Debug.Log("material Index = " + materialIndex);

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            vertexNewColor = textInfo.meshInfo[materialIndex].colors32;
            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
            //Debug.Log("vertex Index = " + vertexIndex);

            // Set all to full alpha
            vertexNewColor[vertexIndex + 0].a = 255;
            vertexNewColor[vertexIndex + 1].a = 255;
            vertexNewColor[vertexIndex + 2].a = 255;
            vertexNewColor[vertexIndex + 3].a = 255;

            lastCharacter = currentCharacter;

            //Debug.Log("Last character end = " + lastCharacter);

            dialogueTexts[currentLine].UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        }

        dialogueTexts[currentLine].UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    public void NextLine()
    {
        currentLine += 1;

        if (currentLine > maxLines - 2)
        {
            isThereAnotherLine = false;
            ending = true;
        }
        else
        {
            isThereAnotherLine = true;
        }

        SetDialogueParameters();

        SetSlideParameters();

        boxReady = false;
    }

    public void EndDialogueAnimation()
    {
        if (currentResetTime < maxResetTime)
        {
            currentResetTime += Time.deltaTime * 1;
        }
        else
        {
            ending = false;
        }

        //Debug.Log("lastY = " + lastY);
        //Debug.Log("Ydisplacment = " + Ydisplacment);

        percentY = resetDialogueCurve.Evaluate(currentResetTime);

        dialogueGo.anchoredPosition = new Vector2(dialogueGo.anchoredPosition.x, lastY + Ydisplacment * (percentY / maxResetTime));
    }

}
