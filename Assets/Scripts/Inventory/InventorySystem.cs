﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventorySystem : MonoBehaviour
{
    public Image inventoryInputImage;
    public Button inventoryButton;
    public RectTransform startPos;
    public bool isInventoryDisplayed = false;
    public bool isAnimationOver = true;

    [Header("Slot")]
    public List<Slot_Behaviour> slotsBehaviour;
    public List<RectTransform> slotsRectTransform;

    public List<Vector2> iconsPosition;
    public List<Vector2> positionDiff;

    [Header("Animation Parameters")]
    public AnimationCurve inventoryApparitionCurve;
    public float apparitionPercent;
    public float apparitionTimeMax;
    public float apparitionCurrentTime;


    private void Awake()
    {
        //Set up les parametres d'animation des icons d'inventaire
        positionDiff.Clear();
        positionDiff = new List<Vector2>();

        for (int i = 0; i < iconsPosition.Count; i++)
        {
            positionDiff.Add(new Vector2());
            positionDiff[i] = new Vector2(startPos.localPosition.x - iconsPosition[i].x, startPos.localPosition.y - iconsPosition[i].y);
        }

        for (int i = 0; i < iconsPosition.Count; i++)
        {
            slotsRectTransform[i].localPosition = startPos.localPosition;
        }
    }

    private void Update()
    {
        if (!isInventoryDisplayed)
        {
            InventoryHide();
        }

        if (isInventoryDisplayed)
        {
            InventoryAppears();
        }
    }

    public void InventoryAnimation()
    {
        isInventoryDisplayed = !isInventoryDisplayed;
        isAnimationOver = false;
        apparitionCurrentTime = 0;
    }

    /// <summary>
    /// Affiche l'inventaire
    /// </summary>
    private void InventoryAppears()
    {
        if (apparitionCurrentTime <= apparitionTimeMax)
        {
            apparitionCurrentTime += Time.deltaTime;
            inventoryButton.interactable = false;
        }
        else
        {
            inventoryButton.interactable = true;
            isAnimationOver = true;
        }

        apparitionPercent = inventoryApparitionCurve.Evaluate(apparitionCurrentTime / apparitionTimeMax);

        slotsRectTransform[0].localPosition = new Vector2(startPos.localPosition.x - (positionDiff[0].x * apparitionPercent), startPos.localPosition.y - (positionDiff[0].y * apparitionPercent));
        slotsRectTransform[1].localPosition = new Vector2(startPos.localPosition.x - (positionDiff[1].x * apparitionPercent), startPos.localPosition.y - (positionDiff[1].y * apparitionPercent));
        slotsRectTransform[2].localPosition = new Vector2(startPos.localPosition.x - (positionDiff[2].x * apparitionPercent), startPos.localPosition.y - (positionDiff[2].y * apparitionPercent));
    }

    /// <summary>
    /// Cache l'inventaire
    /// </summary>
    private void InventoryHide()
    {
        if (apparitionCurrentTime <= apparitionTimeMax)
        {
            apparitionCurrentTime += Time.deltaTime;
            inventoryButton.interactable = false;
        }
        else
        {
            inventoryButton.interactable = true;
            isAnimationOver = true;
        }

        apparitionPercent = inventoryApparitionCurve.Evaluate(apparitionCurrentTime / apparitionTimeMax);

        slotsRectTransform[0].localPosition = new Vector2(iconsPosition[0].x + (positionDiff[0].x * apparitionPercent), iconsPosition[0].y + (positionDiff[0].y * apparitionPercent));
        slotsRectTransform[1].localPosition = new Vector2(iconsPosition[1].x + (positionDiff[1].x * apparitionPercent), iconsPosition[1].y + (positionDiff[1].y * apparitionPercent));
        slotsRectTransform[2].localPosition = new Vector2(iconsPosition[2].x + (positionDiff[2].x * apparitionPercent), iconsPosition[2].y + (positionDiff[2].y * apparitionPercent));
    }

    public void AssignToAvailableSlot(Objet_Interaction collectedObject)
    {
        for (int i = 0; i < slotsBehaviour.Count; i++)
        {
            if(slotsBehaviour[i].isAssigned == false)
            {
                slotsBehaviour[i].uiObjectImage.sprite = collectedObject.objectImage;
                slotsBehaviour[i].uiObjectName.text = collectedObject.objectName;
                slotsBehaviour[i].uiObjectDescritpion.text = collectedObject.objectDescription;
                return;
            }
        }
    }

    public void DropAnObject()
    {

    }
}