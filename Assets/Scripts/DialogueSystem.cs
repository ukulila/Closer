﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEditor;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.IMGUI;

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


    private List<string> lines;
    private char[] currentLineCharacters;
    public List<char[]> linesCharacters;
    private TextMeshProUGUI[] dialogueTexts;
    private GameObject[] dialogueBoxes;
    public Image[] actorsIcon;
    public GameObject dialogueBoxPrefab;

    public List<AnimationCurve> lineTypingSpeed;
    private float currentTime;
    private float maxTime;
    private float typingTimeRatio;
    private float typingSpeedRatio = 1;
    private float typingFasterRatio = 2;
    private int currentCharacter;
    private int lastCharacter;

    public enum Actors { Blanche, Mireille, Louis, MmeBerleau, Dotty, Jolly, Dolores, Maggie, Esdie, Walter, Ray, Barney, Irina };
    private Actors currentActor;
    private List<Actors> actors;


    [Header("Dialogue 'Content'")]
    public int currentLine;
    public int maxLines;

    [Header("Dialogue Box Image Parameters")]
    public float boxesSpacing;
    private float boxMaxWidth = 363;
    private float boxMinWidth = 153;

    private float boxMinHeight = 60;
    private float boxHeigthPerLine = 45;

    private float textHeightPerLine = 33;
    private float textMinHeight = 31;
    private float textWidth = 320;


    private bool isDialogueFinished = true;
    private bool endOfTheLine;
    private bool isThereAnotherLine;



    private void Awake()
    {
        SetDialogueParameters();
        isDialogueFinished = false;

        if (currentLine > maxLines - 1)
        {
            isThereAnotherLine = false;
        }
        else
        {
            isThereAnotherLine = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (endOfTheLine && !isThereAnotherLine)
                isDialogueFinished = true;

            if (endOfTheLine && isThereAnotherLine)
                NextLine();

            if (!endOfTheLine)
                typingTimeRatio = typingFasterRatio;

            if (isDialogueFinished)
                ResetDialogueParameters();

        }

        if (!isDialogueFinished && !endOfTheLine)
            DialogueUpdate();
    }



    #region Dialogue EDITOR SET UP
    public void SetUpTextFile()
    {
        textOfAsset = asset.ToString();

        characters = textOfAsset.ToCharArray();
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

                    /*
                    if (lines.Count != 0)
                    {
                        //lines[lines.Count - 1] = lines[lines.Count - 1].Remove(lines[lines.Count - 1].Length -1);
                    }
                    */
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

            float value = (lines[i].ToCharArray().Length / 10);
            if (value < 1)
                value = 1;

            lineTypingSpeed[i].AddKey(value, lines[i].ToCharArray().Length);

            GameObject currentGO = Instantiate(dialogueBoxPrefab, transform);
            dialogueBoxes.SetValue(currentGO, i);

            dialogueTexts.SetValue(dialogueBoxes[i].GetComponentInChildren<TextMeshProUGUI>(), i);
            dialogueTexts[i].text = "";

            if (actors[i] == Actors.Blanche)
            {
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchorMin = new Vector2(0, 0.5f);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchorMax = new Vector2(0, 0.5f);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().pivot = new Vector2(0, 0.5f);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-435, -135);
            }
            else
            {
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchorMin = new Vector2(1, 0.5f);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchorMax = new Vector2(1, 0.5f);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().pivot = new Vector2(1, 0.5f);
                dialogueBoxes[i].GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-140, -135);
            }

            if (dialogueTexts[i].GetTextInfo(lines[i]).lineCount > 1)
            {
                dialogueTexts[i].GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, textMinHeight + ((dialogueTexts[i].GetTextInfo(lines[i]).lineCount - 1) * textHeightPerLine));
                dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(boxMaxWidth, boxMinHeight + ((dialogueTexts[i].GetTextInfo(lines[i]).lineCount - 1) * boxHeigthPerLine));
            }
            else
            {
                dialogueBoxes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(boxMinWidth, boxMinHeight);
                dialogueTexts[i].GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, textMinHeight);
            }

            //Debug.Log("La ligne comporte " + dialogueTexts[i].GetTextInfo(lines[i]).characterCount + "lettres");
            //Debug.Log("pour un maximum de " + dialogueTexts[i].GetTextInfo(lines[i]).characterInfo.Length);
            //Debug.Log("La ligne "+ i + " peut contenir" + dialogueTexts[i].wordWrappingRatios + " lettres");
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
            DestroyImmediate(dialogueBoxes[i]);
        }

        dialogueBoxes = new GameObject[0];
        dialogueTexts = new TextMeshProUGUI[0];
        lineTypingSpeed.Clear();
        lines.Clear();
        actors.Clear();
        currentWord = "";
    }
    #endregion



    public void ResetDialogueParameters()
    {
        currentTime = 0;
        currentCharacter = 0;
        lastCharacter = -1;
        currentLine = 0;
        typingTimeRatio = typingSpeedRatio;
    }

    public void SetDialogueParameters()
    {
        currentTime = 0;
        currentCharacter = 0;
        lastCharacter = -1;
        typingTimeRatio = typingSpeedRatio;

        maxTime = lineTypingSpeed[currentLine].keys[lineTypingSpeed[currentLine].length - 1].time;

        currentLineCharacters = lines[currentLine].ToCharArray();

        endOfTheLine = false;
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
        }

        currentCharacter = (int)lineTypingSpeed[currentLine].Evaluate(currentTime);

        //Debug.Log("Current Character = " + currentCharacter);
        //Debug.Log("Last Character = " + lastCharacter);

        if (currentCharacter != lastCharacter)
        {
            dialogueTexts[currentLine].text = dialogueTexts[currentLine].text + currentLineCharacters[currentCharacter - 1];
            lastCharacter = currentCharacter;
        }
    }


    public void NextLine()
    {
        currentLine += 1;
        typingTimeRatio = typingSpeedRatio;

        if (currentLine > maxLines - 1)
        {
            isThereAnotherLine = false;
        }
        else
        {
            isThereAnotherLine = true;
        }

        SetDialogueParameters();
    }


}
