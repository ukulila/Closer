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
    public Image[] iconsList;


    public CharacterArray charArray;
    public ListOfCharacterArray listOfCharArray;


    public List<AnimationCurve> lineTypingSpeed;
    public float ratioValue;
    public float currentTime;
    public float maxTime;
    public float typingTimeRatio;
    private float typingSpeedRatio = 1;
    private float typingFasterRatio = 1;
    public int currentCharacter;
    public int lastCharacter;

    public enum Actors { Blanche, Mireille, Louis, MmeBerleau, Dotty, Jolly, Dolores, Maggie, Esdie, Walter, Ray, Barney, Nikky, Irina };
    public Actors currentActor;
    public List<Actors> actors;
    public List<int> activeActorsIndex;
    public GameObject actorsIconHierarchyReference;
    public List<Color> boxColor;
    public List<Color> actorsColor;


    [Header("Dialogue 'Content'")]
    public int currentLine;
    public int maxLines;

    [Header("Dialogue Box Image Parameters")]
    //Parametres BOX des rectTransform
    public float dialogueBoxSpacing = 2000f;
    public float boxMaxWidth = 363f;
    public float boxMinWidth = 152f;

    public float boxMinHeight = 80f;
    public float boxHeigthPerLine = 25f;

    public float boxInitPos_X = -445f;
    public float boxInitPos_Y = -150f;

    public float boxInitPos_X2 = -125f;
    public float boxInitPos_Y2 = -150f;


    //Parametres TEXT des rectTransform
    public float textHeightPerLine = 35f;
    public float textMinHeight = 35f;
    public float textWidth = 320;

    [Header("Writting Parameters")]
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
    public float maxAppearingTime = 1;
    public float appearingPercent;
    public float AppearTimeRatio = 3;

    public Color speakingColor;
    public Color listeningColor;

    //Adapter les boxes à leur text
    public Vector2 textSize;
    public float rendWidth;
    public float rendHeight;




    private void Awake()
    {

        dialogueHasStarted = false;


        if (currentLine == maxLines - 1)
        {
            isThereAnotherLine = false;
            ending = true;
        }
        else
        {
            isThereAnotherLine = true;
        }

        //StartDialogue();
    }


    public void StartDialogue()
    {
        StartCoroutine(StartDialogueIn(2));
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


        if (Input.GetMouseButtonDown(1))
        {
            StartDialogue();
        }


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
        }


        if (isDialogueFinished && ending)
            EndDialogueAnimation();

        if (!boxReady && !isDialogueFinished)
            SlideDialogueTo();

        if (!dialogueBoxReady && !isDialogueFinished)
            DialogueBoxAppear();

        if (isDialogueFinished && !ending && dialogueBoxReady && dialogueHasStarted)
            ResetDialogueParameters();

        if (!isDialogueFinished && !endOfTheLine && writting)
            DialogueUpdate();
    }


    #region Dialogue EDITOR SET UP
    public void SetUp()
    {
        //CleanDialogueSetUp();
        SetUpDefaultValues();
        SetUpActorIcons();
        SetUpActorCalorDefaultValue();
        SetUpTextFile();
        SetUpDialogueLines();
        SetUpDialogueBox();
        SetActorsParameters();
    }

    private void SetUpDefaultValues()
    {
        dialogueBoxSpacing = 130f;
        boxMaxWidth = 363f;
        boxMinWidth = 100f;
        boxMinHeight = 74.63f;
        boxHeigthPerLine = 26.59f;

        boxInitPos_X = -477f;
        boxInitPos_Y = -150f;

        boxInitPos_X2 = -177f;
        boxInitPos_Y2 = -150f;

        //Parametres TEXT des rectTransform
        textHeightPerLine = 26.59f;
        textMinHeight = 34.63f;
        textWidth = 320;

        Ydisplacment = 1900f;
        maxResetTime = 1.5f;

        ratioValue = 10f;
        maxTime = 3f;
        lastCharacter = -1;
        maxSlideTime = 0.9f;

        actors = new List<Actors>();
        lines = new List<string>();
        dialogueBoxes = new GameObject[0];
        dialogueTexts = new TextMeshProUGUI[0];
        lineTypingSpeed = new List<AnimationCurve>();
        boxAppearingCurve = new AnimationCurve();
        boxSlidingCurve = new AnimationCurve();
        resetDialogueCurve = new AnimationCurve();
        translationCount = new List<float>();
        activeActorsIndex = new List<int>();
        dialogueBoxHeights = new List<float>();
        dialogueBoxWidths = new List<float>();
        boxColor = new List<Color>();
        actorsColor = new List<Color>();
    }

    public void AssignActorsIcon()
    {
        if (actorsIconHierarchyReference == null)
        {
            //Debug.Log(actorsIconHierarchyReference.GetComponentsInChildren<Image>().Length);

            for (int i = 0; i < actorsIconHierarchyReference.GetComponentsInChildren<Image>().Length - 1; i++)
            {
                actorsIcon.Add(actorsIconHierarchyReference.GetComponentsInChildren<Image>()[i]);
            }
        }

        if (dialogueGo == null)
        {
            dialogueGo = this.GetComponent<RectTransform>();
        }

        actors = new List<Actors>();
    }

    private void SetUpTextFile()
    {
        textOfAsset = asset.ToString();
        characters = textOfAsset.ToCharArray();
    }

    private void SetUpActorIcons()
    {
        for (int i = 0; i < actorsIconHierarchyReference.transform.childCount; i++)
        {
            actorsIconHierarchyReference.transform.GetChild(i).gameObject.SetActive(true);
        }

        actorsIcon = new List<Image>();
        iconsList = new Image[actorsIconHierarchyReference.GetComponentsInChildren<Image>().Length];

        for (int i = 0; i < iconsList.Length; i++)
        {
            iconsList.SetValue(actorsIconHierarchyReference.GetComponentsInChildren<Image>()[i], i);
            actorsColor.Add(new Color());
        }

        if (iconsList != null)
        {
            for (int i = 0; i < iconsList.Length; i++)
            {
                actorsIcon.Add(iconsList[i]);
                actorsIcon[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetUpActorCalorDefaultValue()
    {

    }

    private void SetUpDialogueLines()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (char.IsWhiteSpace(characters[i]))
            {
                spaceCount++;


                //Vérifie si 2 espaces/sauts de ligne consecutifs existent marquant la fin d'une ligne
                if (spaceCount == lineSpace)
                {
                    spaceCount = 0;
                    actorSet = false;
                    write = false;

                    /*
                    char[] charArrayTemporary = lines[lines.Count - 1].ToCharArray();
                    charArrayTemporary.SetValue(null, charArrayTemporary.Length);
                    lines[lines.Count - 1] = charArrayTemporary.ArrayToString();
                    */
                }

                //Vérifie à quelle actor correspond le premier mot
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

                currentWord = "";
            }
            else
            {
                spaceCount = 0;

                //Une fois l'actor set up, on assigne la suite à sa ligne de dialogue
                if (actorSet)
                    write = true;

                currentWord = currentWord + characters[i];
            }

            //Ecriture d'une ligne de dialogue
            if (write)
            {
                lines[lines.Count - 1] = lines[lines.Count - 1] + characters[i];
            }
        }
    }

    private void SetUpDialogueBox()
    {
        //Set le nombre de lignes
        maxLines = lines.Count;

        //Set les list au nombre de ligne
        dialogueBoxes = new GameObject[maxLines];
        dialogueTexts = new TextMeshProUGUI[maxLines];
        lineTypingSpeed = new List<AnimationCurve>();
        boxAppearingCurve = new AnimationCurve();
        boxSlidingCurve = new AnimationCurve();
        resetDialogueCurve = new AnimationCurve();
        boxColor = new List<Color>();



        boxAppearingCurve.AddKey(0, 0);
        boxSlidingCurve.AddKey(0, 0);
        resetDialogueCurve.AddKey(0, 0);

        boxAppearingCurve.AddKey(1, 1);
        boxSlidingCurve.AddKey(1, 1);
        resetDialogueCurve.AddKey(1, 1);
        //listOfCharArray.lines = new List<CharacterArray>();

        //Set des animations curve par défault
        for (int i = 0; i < maxLines; i++)
        {
            lineTypingSpeed.Add(new AnimationCurve());
            lineTypingSpeed[i].AddKey(0, 0);


            /*
            charArray.charactersArray = lines[i].ToCharArray();
            CharacterArray array = new CharacterArray();
            array.charactersArray = lines[i].ToCharArray();
            listOfCharArray.lines.Add(array);
            */


            float value = (lines[i].ToCharArray().Length / ratioValue);

            if (value < 1)
                value = 1;

            lineTypingSpeed[i].AddKey(value, lines[i].ToCharArray().Length);

            //Pour chaque ligne, on crée un instance du prefab dialogueBOX
            GameObject currentGO = Instantiate(dialogueBoxPrefab, transform);
            //Et on assigne le GameObject instancié dans une liste
            dialogueBoxes.SetValue(currentGO, i);
            //Et on assigne le TextMeshProGUI du GameObject instancié dans une autre liste
            dialogueTexts.SetValue(dialogueBoxes[i].GetComponentInChildren<TextMeshProUGUI>(), i);

            //Enfin on assigne au text du TextMeshProGUI du GameObject instancié la string de chaque ligne
            dialogueTexts[i].text = lines[i];


            //Modification des transforms selon l'actor (Blanche à droite et les autres à gauche)
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


            //Modification des transforms (width et height) du dialogueBox selon son contenu
            if (dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount > 1)
            {
                dialogueTexts[i].GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, textMinHeight +
                    ((dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount - 1) * textHeightPerLine) + ((dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount - 1) * 4.46f));

                dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(boxMaxWidth, boxMinHeight +
                    ((dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount - 1) * boxHeigthPerLine) + ((dialogueTexts[i].GetTextInfo(dialogueTexts[i].text).lineCount - 1) * 4.46f));
            }
            else
            {
                rendWidth = dialogueTexts[i].renderedWidth;

                float widthPercent = rendWidth / boxMaxWidth;

                dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(boxMinWidth + (widthPercent * (boxMaxWidth - (boxMinWidth - 25f))), boxMinHeight);
                dialogueTexts[i].GetComponent<RectTransform>().sizeDelta = new Vector2(rendWidth, textMinHeight);
            }


        }

        //Placement des différentes dialogueBox dans l'espace par rapport au uns et aux autres
        for (int i = 1; i < maxLines; i++)
        {

            float posYdiff = ((dialogueBoxes[i].GetComponent<RectTransform>().anchoredPosition.y - dialogueBoxes[i].GetComponent<RectTransform>().offsetMax.y) +
                (dialogueBoxes[i - 1].GetComponent<RectTransform>().offsetMin.y - dialogueBoxes[i - 1].GetComponent<RectTransform>().anchoredPosition.y)) - dialogueBoxSpacing;

            translationCount.Add(posYdiff);

            dialogueBoxes[i].GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(dialogueBoxes[i].GetComponent<RectTransform>().anchoredPosition.x, (dialogueBoxes[i - 1].GetComponent<RectTransform>().anchoredPosition.y + translationCount[i - 1]));
        }

        //Assignation des valeurs (width et height) de chaque dialogueBox dans 2 listes distincts puis reset de ces dernières en 0 pour la futur animation d'apparition du dialogueBox
        for (int i = 0; i < maxLines; i++)
        {
            dialogueBoxHeights.Add(dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta.y);
            dialogueBoxWidths.Add(dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta.x);

            boxColor.Add(dialogueBoxes[i].GetComponent<Image>().color);

            dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }
    }

    //Nettoyage de toutes les valeurs selon le text assigné
    public void CleanDialogueSetUp()
    {
        textOfAsset = "";
        characters = new char[0];
        actorSet = false;
        write = false;

        if (dialogueBoxes.Length != 0)
        {
            for (int i = 0; i < dialogueBoxes.Length; i++)
            {
                if (dialogueBoxes[i] != null)
                    DestroyImmediate(dialogueBoxes[i]);
            }
        }

        dialogueBoxes = new GameObject[0];
        dialogueTexts = new TextMeshProUGUI[0];

        if (lineTypingSpeed.Count != 0)
            lineTypingSpeed.Clear();

        lines.Clear();
        actors.Clear();
        dialogueGo.position = new Vector2(dialogueGo.position.x, dialogueGo.position.y);
        translationCount.Clear();
        activeActorsIndex.Clear();
        dialogueBoxHeights.Clear();
        dialogueBoxWidths.Clear();
        boxColor.Clear();
        actorsColor.Clear();

        //listOfCharacterArray.lines.Clear();
        currentWord = "";
    }

    private void SetActorsParameters()
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

    private void DesactivateIcons()
    {
        for (int i = 0; i < activeActorsIndex.Count; i++)
        {
            actorsIcon[(activeActorsIndex[i])].gameObject.SetActive(false);
        }
    }
    #endregion


    private void ActivateActorsIcons()
    {
        for (int i = 0; i < activeActorsIndex.Count; i++)
        {
            actorsIcon[activeActorsIndex[i]].gameObject.SetActive(true);
        }

        SetDialogueParameters();
    }

    private void SlideDialogueTo()
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


    private void ResetDialogueParameters()
    {
        currentTime = 0;
        currentSlideTime = 0;
        currentCharacter = 0;
        lastCharacter = -1;
        currentLine = 0;
        currentAppearingTime = 0;
        typingTimeRatio = typingSpeedRatio;

        SetUp();

        dialogueGo.anchoredPosition = new Vector2(0, 0);

        ROOM_Manager.Instance.LaunchUI();

        endOfTheLine = false;
        writting = false;
        ending = false;
        boxReady = true;
        dialogueBoxReady = false;
        dialogueHasStarted = false;
    }

    private void SetDialogueParameters()
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


    private void DialogueUpdate()
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

    private void NextLine()
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

    private void EndDialogueAnimation()
    {
        if (currentResetTime < maxResetTime)
        {
            currentResetTime += Time.deltaTime * 1;
        }
        else
        {
            DesactivateIcons();
            ROOM_Manager.Instance.LaunchUI();
            ending = false;
        }

        //Debug.Log("lastY = " + lastY);
        //Debug.Log("Ydisplacment = " + Ydisplacment);

        percentY = resetDialogueCurve.Evaluate(currentResetTime);

        dialogueGo.anchoredPosition = new Vector2(dialogueGo.anchoredPosition.x, lastY + Ydisplacment * (percentY - maxResetTime));

        float numberOfActors = activeActorsIndex.Count;

        if (numberOfActors == 1)
        {
            actorsIcon[activeActorsIndex[0]].color = new Color32((byte)actorsIcon[activeActorsIndex[0]].color.r, (byte)actorsIcon[activeActorsIndex[0]].color.g,
            (byte)actorsIcon[activeActorsIndex[0]].color.b, (byte)(actorsIcon[activeActorsIndex[0]].color.a - 255 * (percentY - maxResetTime)));
        }

        if (numberOfActors == 2)
        {
            actorsIcon[activeActorsIndex[0]].color = new Color32((byte)actorsIcon[activeActorsIndex[0]].color.r, (byte)actorsIcon[activeActorsIndex[0]].color.g,
            (byte)actorsIcon[activeActorsIndex[0]].color.b, (byte)(actorsIcon[activeActorsIndex[0]].color.a - 255 * (percentY - maxResetTime)));

            actorsIcon[activeActorsIndex[1]].color = new Color32((byte)actorsIcon[activeActorsIndex[1]].color.r, (byte)actorsIcon[activeActorsIndex[1]].color.g,
            (byte)actorsIcon[activeActorsIndex[1]].color.b, (byte)(actorsIcon[activeActorsIndex[1]].color.a - 255 * (percentY - maxResetTime)));
        }

        if (numberOfActors == 3)
        {
            actorsIcon[activeActorsIndex[0]].color = new Color32((byte)actorsIcon[activeActorsIndex[0]].color.r, (byte)actorsIcon[activeActorsIndex[0]].color.g,
            (byte)actorsIcon[activeActorsIndex[0]].color.b, (byte)(actorsIcon[activeActorsIndex[0]].color.a - 255 * (percentY - maxResetTime)));

            actorsIcon[activeActorsIndex[1]].color = new Color32((byte)actorsIcon[activeActorsIndex[1]].color.r, (byte)actorsIcon[activeActorsIndex[1]].color.g,
            (byte)actorsIcon[activeActorsIndex[1]].color.b, (byte)(actorsIcon[activeActorsIndex[1]].color.a - 255 * (percentY - maxResetTime)));

            actorsIcon[activeActorsIndex[2]].color = new Color32((byte)actorsIcon[activeActorsIndex[2]].color.r, (byte)actorsIcon[activeActorsIndex[2]].color.g,
            (byte)actorsIcon[activeActorsIndex[2]].color.b, (byte)(actorsIcon[activeActorsIndex[2]].color.a - 255 * (percentY - maxResetTime)));
        }

        if (numberOfActors == 4)
        {
            actorsIcon[activeActorsIndex[0]].color = new Color32((byte)actorsIcon[activeActorsIndex[0]].color.r, (byte)actorsIcon[activeActorsIndex[0]].color.g,
            (byte)actorsIcon[activeActorsIndex[0]].color.b, (byte)(actorsIcon[activeActorsIndex[0]].color.a - 255 * (percentY - maxResetTime)));

            actorsIcon[activeActorsIndex[1]].color = new Color32((byte)actorsIcon[activeActorsIndex[1]].color.r, (byte)actorsIcon[activeActorsIndex[1]].color.g,
            (byte)actorsIcon[activeActorsIndex[1]].color.b, (byte)(actorsIcon[activeActorsIndex[1]].color.a - 255 * (percentY - maxResetTime)));

            actorsIcon[activeActorsIndex[2]].color = new Color32((byte)actorsIcon[activeActorsIndex[2]].color.r, (byte)actorsIcon[activeActorsIndex[2]].color.g,
            (byte)actorsIcon[activeActorsIndex[2]].color.b, (byte)(actorsIcon[activeActorsIndex[2]].color.a - 255 * (percentY - maxResetTime)));

            actorsIcon[activeActorsIndex[3]].color = new Color32((byte)actorsIcon[activeActorsIndex[3]].color.r, (byte)actorsIcon[activeActorsIndex[3]].color.g,
            (byte)actorsIcon[activeActorsIndex[3]].color.b, (byte)(actorsIcon[activeActorsIndex[3]].color.a - 255 * (percentY - maxResetTime)));
        }

    }

}
