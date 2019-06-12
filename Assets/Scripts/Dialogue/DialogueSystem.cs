using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("DEBUG bool")]
    public bool startOnAwake;

    [Header("***** Options")]
    public bool isForCinematic;
    public bool doYouHaveFade;
    public bool isLastDialogue;
    public bool isNpcDesappearingAfter;

    [Header("Contenu en lignes du Dialogue")]
    public TextAsset asset;
    public int currentLine;
    public int maxLines;
    [Space]
    private string textOfAsset;
    private char[] characters;
    private string currentWord;
    private int spaceCount;
    private int lineSpace = 2;
    private bool actorSet = false;
    private bool write = false;

    [Header("   Répartition et Assignation du Contenu")]
    public List<string> lines;
    public char[] currentLineCharacters;
    public TextMeshProUGUI[] dialogueTexts;
    public GameObject[] dialogueBoxes;
    public GameObject dialogueBoxPrefab;
    public TMP_Vertex vertex;
    public List<Image> actorsIcon;
    public Image[] iconsList;

    /*
    public CharacterArray charArray;
    public ListOfCharacterArray listOfCharArray;
    */

    [Header("   Vitesse d'écriture")]
    public List<AnimationCurve> lineTypingSpeed;
    public float ratioValue;
    public float currentTime;
    public float maxTime;
    public float typingTimeRatio;
    private float typingSpeedRatio = 2.5f;
    private float typingFasterRatio = 3.5f;
    public int currentCharacter;
    public int lastCharacter;


    [Header("   Récupération des acteurs")]
    public GameObject actorsIconHierarchyReference;
    public enum Actors { Blanche, Mireille, Mehdi, MmeBerleau, Dotty, Jolly, Dolores, Maggie, Esdie, Walter, Ray, Barney, Nikky, Irina };
    public Actors currentActor;
    public List<Actors> actors;
    public List<int> activeActorsIndex;




    [Header("Dialogue Box Image Parameters")]
    //Parametres BOX des rectTransform
    public float dialogueBoxSpacing = 2000f;
    public float boxMaxWidth = 363f;
    public float boxMinWidth = 152f;
    [Space]
    public float boxMinHeight = 80f;
    public float boxHeigthPerLine = 25f;
    [Space]
    public float boxInitPos_X = -480f;
    public float boxInitPos_Y = -150f;
    [Space]
    public float boxInitPos_X2 = -125f;
    public float boxInitPos_Y2 = -150f;
    [Space]
    //Adapter les boxes à leur text
    public Vector2 textSize;
    public float rendWidth;
    public float rendHeight;
    [Space]
    //Parametres TEXT des rectTransform
    public float textHeightPerLine = 35f;
    public float textMinHeight = 35f;
    public float textWidth = 320;

    [Header("Writting Parameters")]
    //Différents etats du Dialogue
    public bool isDialogueFinished = false;
    public bool endOfTheLine;
    public bool writting;
    public bool isThereAnotherLine;
    public bool ending;
    public bool dialogueHasStarted;
    public bool isFaster;


    //Changer la couleur character per character
    public TMP_Text m_TextComponent;
    public TMP_TextInfo textInfo;
    public Color32[] vertexNewColor;

    [Header("Box Sliding Set Up Parameters")]
    public List<float> translationCount;
    public RectTransform dialogueGo;
    public bool boxReady = true;
    public Vector2 nextLinePosition;

    [Header("   Box Sliding Animation")]
    public AnimationCurve boxSlidingCurve;
    public float currentSlideTime;
    public float maxSlideTime;
    public float currentSlidePercent;
    public float currentYPos;
    public float nextYPos;

    [Header("   Start Dialogue Animation")]
    public AnimationCurve startCurve;
    public float currentStartTime;
    public float maxStartTime;
    public float currentStartPercent;

    public Image nameBox;
    public TextMeshProUGUI nameCharacter;

    public bool isStarting = false;

    [Header("   Dialogue End Animation")]
    public AnimationCurve resetDialogueCurve;
    public float currentResetTime;
    public float maxResetTime;
    [Space]
    public float lastY;
    public float percentY;
    public float Ydisplacment;
    [Space]
    public List<float> dialogueBoxWidths;
    public List<float> dialogueBoxHeights;
    public bool dialogueBoxReady = true;

    [Header("   Box Apparition Animation")]
    public AnimationCurve boxAppearingCurve;
    public float currentAppearingTime;
    public float maxAppearingTime = 1;
    public float appearingPercent;
    public float AppearTimeRatio = 5;

    [Header("   Box Color during dialogue")]
    public Color speakingColor;
    public Color listeningColor;
    public List<Color> actorsColorReference;

    [Header("Actors Box Color")]
    public Color32 blancheColor;
    public Color32 mireilleColor;
    public Color32 louisColor;
    public Color32 berleauColor;
    public Color32 dottyColor;
    public Color32 jollyColor;
    public Color32 doloresColor;
    public Color32 maggieColor;
    public Color32 esdieColor;
    public Color32 walterColor;
    public Color32 rayColor;
    public Color32 barneyColor;
    public Color32 nikkyColor;
    public Color32 irinaColor;

    [Header("Actors name")]
    public List<string> names;





    private void Awake()
    {
        dialogueHasStarted = false;

        if (currentLine == maxLines)
            ending = true;

        if (currentLine == maxLines - 1)
        {
            isThereAnotherLine = false;
        }
        else
        {
            isThereAnotherLine = true;
        }

        if (startOnAwake)
            StartDialogue();
    }


    public void StartDialogue()
    {
        CellPlacement.Instance.doOnce = true;

        StartCoroutine(StartDialogueIn(1.5f));
    }

    public void StartingDialogueCurve()
    {
        if (currentStartTime < maxStartTime)
        {
            currentStartTime += Time.deltaTime;
        }
        else
        {
            isStarting = false;
        }

        currentStartPercent = startCurve.Evaluate(currentStartTime / maxStartTime);

        if (isForCinematic)
        {
            nameBox.color = new Color(1, 1, 1, (0 + (1 * currentStartPercent)));
            nameCharacter.color = new Color(1, 1, 1, (0 + (1 * currentStartPercent)));
        }


        float numberOfActors = activeActorsIndex.Count;

        if (numberOfActors == 1 && isForCinematic)
        {
            actorsIcon[activeActorsIndex[0]].color = new Color(1, 1, 1, (0 + (1 * currentStartPercent)));
        }

        if (numberOfActors == 2)
        {
            if (isForCinematic)
                actorsIcon[activeActorsIndex[0]].color = new Color(1, 1, 1, (0 + (1 * currentStartPercent)));

            actorsIcon[activeActorsIndex[1]].color = new Color(1, 1, 1, (0 + (1 * currentStartPercent)));
        }

        /*
        if (numberOfActors == 3)
        {
            actorsIcon[activeActorsIndex[0]].color = new Color32(255, 255, 255, (byte)(0 + 255 * currentStartPercent));

            actorsIcon[activeActorsIndex[1]].color = new Color32(255, 255, 255, (byte)(0 + 255 * currentStartPercent));

            actorsIcon[activeActorsIndex[2]].color = new Color32(255, 255, 255, (byte)(0 + 255 * currentStartPercent));
        }

        if (numberOfActors == 4)
        {
            actorsIcon[activeActorsIndex[0]].color = new Color32(255, 255, 255, (byte)(0 + 255 * currentStartPercent));

            actorsIcon[activeActorsIndex[1]].color = new Color32(255, 255, 255, (byte)(0 + 255 * currentStartPercent));

            actorsIcon[activeActorsIndex[2]].color = new Color32(255, 255, 255, (byte)(0 + 255 * currentStartPercent));

            actorsIcon[activeActorsIndex[3]].color = new Color32(255, 255, 255, (byte)(0 + 255 * currentStartPercent));
        }
        */
    }

    /// <summary>
    /// Démarer le dialogue après un delai dès l'activation du GameObject
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator StartDialogueIn(float time)
    {
        ActivateActorsIcons();

        if (activeActorsIndex.Count != 1)
            nameCharacter.text = names[activeActorsIndex[1]];

        isStarting = true;

        yield return new WaitForSeconds(time);

        isDialogueFinished = false;

        SetDialogueParameters();

        dialogueBoxReady = false;
        dialogueHasStarted = true;

        if (currentLine == maxLines)
            ending = true;

        if (currentLine == maxLines - 1)
        {
            isThereAnotherLine = false;
            //ending = true;
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

        if (!dialogueHasStarted && isStarting)
        {
            StartingDialogueCurve();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (endOfTheLine && !isThereAnotherLine)
            {
                isFaster = false;
                isDialogueFinished = true;
            }


            if (endOfTheLine && isThereAnotherLine && boxReady)
            {
                endOfTheLine = false;
                isFaster = false;
                boxReady = false;

                NextLine();
            }

            if (!endOfTheLine && isFaster && boxReady)
            {
                dialogueTexts[currentLine].color = new Color32(82, 29, 80, 255);
                endOfTheLine = true;
                isFaster = false;
            }

            if (!endOfTheLine && !isFaster && boxReady && dialogueHasStarted)
            {
                typingTimeRatio = typingFasterRatio;
                isFaster = true;
            }
        }


        if (isDialogueFinished && ending)
            EndDialogueAnimation();

        if (!boxReady && !isDialogueFinished)
            SlideDialogueTo();

        if (!dialogueBoxReady && !isDialogueFinished)
            DialogueBoxAppear();

        if (isDialogueFinished && !ending && dialogueBoxReady && dialogueHasStarted)
            ResetDialogueParameters();

        if (!isDialogueFinished && !endOfTheLine && writting && boxReady)
            DialogueUpdate();
    }


    #region Dialogue EDITOR SET UP
    public void SetUp()
    {
        //CleanDialogueSetUp();
        SetUpDefaultValues();
        SetUpActorIcons();
        SetUpCharactersName();
        SetUpDefaultActorsColor();
        SetUpTextFile();
        SetUpDialogueLines();
        SetUpDialogueBox();
        SetActorsParameters();
    }

    private void SetUpDefaultActorsColor()
    {
        actorsColorReference[0] = blancheColor;
        actorsColorReference[1] = mireilleColor;
        actorsColorReference[2] = louisColor;
        actorsColorReference[3] = berleauColor;
        actorsColorReference[4] = dottyColor;
        actorsColorReference[5] = jollyColor;
        actorsColorReference[6] = doloresColor;
        actorsColorReference[7] = maggieColor;
        actorsColorReference[8] = esdieColor;
        actorsColorReference[9] = walterColor;
        actorsColorReference[10] = rayColor;
        actorsColorReference[11] = barneyColor;
        actorsColorReference[12] = nikkyColor;
        actorsColorReference[13] = irinaColor;
    }

    private void SetUpDefaultValues()
    {
        dialogueBoxSpacing = 30f;
        boxMaxWidth = 363f;
        boxMinWidth = 100f;
        boxMinHeight = 80f; //74.63f
        boxHeigthPerLine = 30f; //26.59f

        boxInitPos_X = -480f;
        boxInitPos_Y = -150f;

        boxInitPos_X2 = -177f;
        boxInitPos_Y2 = -150f;

        //Parametres TEXT des rectTransform
        textHeightPerLine = 30f; //26.59f
        textMinHeight = 34.63f; //34.63f
        textWidth = 315f;

        Ydisplacment = 1900f;
        maxResetTime = 1;

        ratioValue = 10f;
        maxTime = 3f;
        lastCharacter = -1;
        maxSlideTime = 0.5f;

        AppearTimeRatio = 5;

        currentStartTime = 0f;
        maxStartTime = 1f;

        actors = new List<Actors>();
        lines = new List<string>();
        dialogueBoxes = new GameObject[0];
        dialogueTexts = new TextMeshProUGUI[0];
        lineTypingSpeed = new List<AnimationCurve>();
        boxAppearingCurve = new AnimationCurve();
        boxSlidingCurve = new AnimationCurve();
        startCurve = new AnimationCurve();
        resetDialogueCurve = new AnimationCurve();
        translationCount = new List<float>();
        activeActorsIndex = new List<int>();
        dialogueBoxHeights = new List<float>();
        dialogueBoxWidths = new List<float>();
        actorsColorReference = new List<Color>();

        blancheColor = new Color32(255, 255, 255, 255);
        mireilleColor = new Color32(241, 172, 90, 255);
        louisColor = new Color32(200, 99, 166, 255); ;
        berleauColor = new Color32(255, 255, 255, 255);
        dottyColor = new Color32(191, 148, 205, 255);
        jollyColor = new Color32(255, 255, 255, 255);
        doloresColor = new Color32(255, 144, 126, 255);
        maggieColor = new Color32(217, 112, 132, 255);
        esdieColor = new Color32(133, 219, 125, 255);
        walterColor = new Color32(196, 145, 224, 255);
        rayColor = new Color32(204, 149, 130, 255);
        barneyColor = new Color32(166, 244, 246, 255);
        nikkyColor = new Color32(161, 236, 196, 255);
        irinaColor = new Color32(158, 218, 231, 255);
    }

    private void SetUpCharactersName()
    {
        names = new List<string>();

        names.Add("Blanche");
        names.Add("Mireille");
        names.Add("Louis");
        names.Add("Madame BERLEAU");
        names.Add("Dotty");
        names.Add("Jolly");
        names.Add("Dolores");
        names.Add("Maggie");
        names.Add("Esdie");
        names.Add("Walter");
        names.Add("Ray");
        names.Add("Barney");
        names.Add("Nikky");
        names.Add("Irina");

    }

    public void AssignActorsIcon()
    {
        if (actorsIconHierarchyReference == null)
        {
            for (int i = 0; i < actorsIconHierarchyReference.GetComponentsInChildren<Image>().Length; i++)
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
            actorsColorReference.Add(new Color());
        }

        //SetUpDefaultActorsColor();

        if (iconsList != null)
        {
            for (int i = 0; i < iconsList.Length; i++)
            {
                actorsIcon.Add(iconsList[i]);

                if (i > 0)
                    actorsIcon[i].gameObject.SetActive(false);
            }
        }
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

                        if (currentWord == Actors.Mehdi.ToString().ToUpper())
                        {
                            actors.Add(Actors.Mehdi);
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
        //actorsColorReference = new List<Color>();

        startCurve.AddKey(0, 0);
        boxAppearingCurve.AddKey(0, 0);
        boxSlidingCurve.AddKey(0, 0);
        resetDialogueCurve.AddKey(0, 0);

        startCurve.AddKey(1, 1);
        boxAppearingCurve.AddKey(1, 1);
        boxSlidingCurve.AddKey(1, 1);
        resetDialogueCurve.AddKey(1, 1);

        //Set des animations curve par défault
        for (int i = 0; i < maxLines; i++)
        {
            lineTypingSpeed.Add(new AnimationCurve());
            lineTypingSpeed[i].AddKey(0, 0);

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

            dialogueBoxes[i].GetComponent<Image>().color = actorsColorReference[(int)actors[i]];

            dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// Nettoyage de toutes les valeurs selon le text assigné
    /// </summary>
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
        actorsColorReference.Clear();
        names.Clear();

        currentWord = "";
    }

    private void SetActorsParameters()
    {
        for (int i = 0; i < actors.Count; i++)
        {
            if (actors[i] == Actors.Blanche)
            {
                actorsIcon[(int)Actors.Blanche].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Mireille)
            {
                actorsIcon[(int)Actors.Mireille].gameObject.SetActive(true);
            }

            if (actors[i] == Actors.Mehdi)
            {
                actorsIcon[(int)Actors.Mehdi].gameObject.SetActive(true);
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

        for (int i = 0; i < actorsIcon.Count; i++)
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
            if (i > 0)
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

        currentSlidePercent = boxSlidingCurve.Evaluate(currentSlideTime / maxSlideTime);

        dialogueGo.anchoredPosition = new Vector2(dialogueGo.anchoredPosition.x, currentYPos + nextYPos * (currentSlidePercent));
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
        currentResetTime = 0;
        lastCharacter = -1;
        currentLine = 0;
        currentAppearingTime = 0;
        typingTimeRatio = typingSpeedRatio;

        dialogueGo.anchoredPosition = new Vector2(335, 72);

        if (!isLastDialogue)
        {
            if (!isForCinematic)
            {
                if (doYouHaveFade)
                {
                    if (NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.currentDialogueIndex].questHasBeenAsked == false)
                    {
                        FadeScript.Instance.FadeINandOUT();

                        //Debug.Log("INVOKE");

                        NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.currentDialogueIndex].questHasBeenAsked = true;

                        StartCoroutine(NPC_Manager.Instance.StartInvokeIn(1.5f));

                        if (isNpcDesappearingAfter)
                        {
                            ROOM_Manager.Instance.LaunchUI(1.5f);
                        }
                        else
                        {
                            StartCoroutine(NPC_Manager.Instance.ReturnToDialogueOPTIONAnimIn(3));
                        }

                        //Debug.Log("current = " + NPC_Manager.Instance.currentNPC.currentDialogueIndex);
                    }
                    else
                    {
                        //Debug.Log("current = " + NPC_Manager.Instance.currentNPC.currentDialogueIndex);
                        //Debug.Log("Nothing to INVOKED " + NPC_Manager.Instance.currentNPC.currentDialogueIndex);
                        StartCoroutine(NPC_Manager.Instance.ReturnToDialogueOPTIONAnimIn(1f));
                    }
                }
                else
                {
                    if (NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.currentDialogueIndex].questHasBeenAsked == false)
                    {
                        //Debug.Log("INVOKE");

                        NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.currentDialogueIndex].questHasBeenAsked = true;

                        StartCoroutine(NPC_Manager.Instance.StartInvokeIn(0.2f));


                        if (isNpcDesappearingAfter)
                        {
                            ROOM_Manager.Instance.LaunchUI(1.5f);
                        }
                        else
                        {
                            StartCoroutine(NPC_Manager.Instance.ReturnToDialogueOPTIONAnimIn(1.5f));
                        }

                        //Debug.Log("current = " + NPC_Manager.Instance.currentNPC.currentDialogueIndex);
                    }
                    else
                    {
                        //Debug.Log("current = " + NPC_Manager.Instance.currentNPC.currentDialogueIndex);
                        //Debug.Log("Nothing to INVOKED " + NPC_Manager.Instance.currentNPC.currentDialogueIndex);
                        StartCoroutine(NPC_Manager.Instance.ReturnToDialogueOPTIONAnimIn(1f));
                    }
                }

                actorsIcon[activeActorsIndex[0]].color = speakingColor;
            }
            else
            {
                if (doYouHaveFade)
                {
                    if (NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.currentDialogueIndex].questHasBeenAsked == false)
                    {
                        FadeScript.Instance.FadeINandOUT();

                        NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.currentDialogueIndex].questHasBeenAsked = true;

                        StartCoroutine(NPC_Manager.Instance.StartInvokeIn(1.5f));

                        StartCoroutine(CinematicTrigger.Instance.DelayBoforeEndingCinematic(2.2f));
                    }
                    else
                    {
                        StartCoroutine(CinematicTrigger.Instance.DelayBoforeEndingCinematic(1.5f));
                    }
                }
                else
                {
                    if (NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.currentDialogueIndex].questHasBeenAsked == false)
                    {
                        NPC_Manager.Instance.currentNPC.dialogue[NPC_Manager.Instance.currentNPC.currentDialogueIndex].questHasBeenAsked = true;

                        StartCoroutine(NPC_Manager.Instance.StartInvokeIn(0f));

                        StartCoroutine(CinematicTrigger.Instance.DelayBoforeEndingCinematic(1f));

                        //CinematicTrigger.Instance.EndCinematicNOW();
                    }
                    else
                    {
                        StartCoroutine(CinematicTrigger.Instance.DelayBoforeEndingCinematic(0.1f));
                    }
                }
            }
        }
        else
        {
            Debug.Log("current = " + NPC_Manager.Instance.currentNPC.currentDialogueIndex);

            NPC_Manager.Instance.onReverse = true;
            NPC_Manager.Instance.isCurveNeeded = true;

            StartCoroutine(NPC_Manager.Instance.StartInvokeIn(1.5f));
        }



        endOfTheLine = false;
        writting = false;
        ending = false;
        boxReady = true;
        dialogueBoxReady = false;
        dialogueHasStarted = false;

        CleanDialogueSetUp();
        SetUp();
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

        if (activeActorsIndex.Count != 1)
        {
            if ((int)currentActor == activeActorsIndex[0])
            {
                actorsIcon[activeActorsIndex[0]].color = speakingColor;
                actorsIcon[activeActorsIndex[1]].color = listeningColor;
            }
            else
            {
                actorsIcon[activeActorsIndex[1]].color = speakingColor;
                actorsIcon[activeActorsIndex[0]].color = listeningColor;
            }
        }
        else
        {
            actorsIcon[activeActorsIndex[0]].color = speakingColor;
        }


    }


    /// <summary>
    /// Parametres de déplacement des boxes
    /// </summary>
    private void SetSlideParameters()
    {
        currentYPos = dialogueGo.anchoredPosition.y;
        nextLinePosition = new Vector2(dialogueGo.anchoredPosition.x, currentYPos - translationCount[currentLine - 1]);
        nextYPos = nextLinePosition.y - currentYPos;

        currentSlideTime = 0;
        currentAppearingTime = 0;
    }


    private void DialogueUpdate()
    {
        if (currentTime < maxTime)
        {
            currentTime += (Time.deltaTime * typingTimeRatio);
        }
        else
        {
            endOfTheLine = true;
            writting = false;
            typingTimeRatio = typingSpeedRatio;

            dialogueTexts[currentLine].color = new Color32(82, 29, 80, 255);
        }

        currentCharacter = (int)lineTypingSpeed[currentLine].Evaluate(currentTime);


        if (currentCharacter != lastCharacter && currentCharacter < textInfo.characterCount)
        {
            int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

            vertexNewColor = textInfo.meshInfo[materialIndex].colors32;

            int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

            vertexNewColor[vertexIndex + 0].a = 255;
            vertexNewColor[vertexIndex + 1].a = 255;
            vertexNewColor[vertexIndex + 2].a = 255;
            vertexNewColor[vertexIndex + 3].a = 255;

            lastCharacter = currentCharacter;


            dialogueTexts[currentLine].UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        }

        //dialogueTexts[currentLine].UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
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
            currentResetTime += Time.deltaTime;
        }
        else
        {
            DesactivateIcons();
            ending = false;
        }

        percentY = resetDialogueCurve.Evaluate(currentResetTime);

        dialogueGo.anchoredPosition = new Vector2(dialogueGo.anchoredPosition.x, lastY + Ydisplacment * percentY);

        if (isForCinematic || isLastDialogue)
        {
            nameBox.color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));
            nameCharacter.color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));
        }



        float numberOfActors = activeActorsIndex.Count;

        if (numberOfActors == 1 && isForCinematic || isLastDialogue)
        {
            actorsIcon[activeActorsIndex[0]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));
        }

        if (numberOfActors == 2)
        {
            if (isForCinematic || isLastDialogue)
                actorsIcon[activeActorsIndex[0]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));

            actorsIcon[activeActorsIndex[1]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));
        }

        if (numberOfActors == 3)
        {
            if (isForCinematic || isLastDialogue)
                actorsIcon[activeActorsIndex[0]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));

            actorsIcon[activeActorsIndex[1]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));

            actorsIcon[activeActorsIndex[2]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));
        }

        if (numberOfActors == 4)
        {
            if (isForCinematic || isLastDialogue)
                actorsIcon[activeActorsIndex[0]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));

            actorsIcon[activeActorsIndex[1]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));

            actorsIcon[activeActorsIndex[2]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));

            actorsIcon[activeActorsIndex[3]].color = new Color32(255, 255, 255, (byte)(255 - 255 * percentY));
        }

    }
}
