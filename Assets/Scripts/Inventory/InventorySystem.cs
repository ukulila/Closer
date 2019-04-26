using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventorySystem : MonoBehaviour
{
    public GameObject inventoryPos;
    
    public bool isInventoryDisplayed = false;
    public bool isAnimationOver = true;

    [Header("Slot")]
    public List<Image> inventoryIconsImage;
    public List<Button> inventoryIconsInteractable;
    public List<Objet_Interaction> inventoryObjets;
    public RectTransform startPos;
    public List<Vector2> iconsPosition;
    public List<Vector2> positionDiff;

    [Header("Animation Parameters")]
    public AnimationCurve apparitionCurve;
    public float apparitionPercent;
    public float apparitionTimeMax;
    public float apparitionCurrentTime;


    private void Awake()
    {
        //Set up les parametres d'animation des icons d'inventaire
        positionDiff.Clear();
        positionDiff = new List<Vector2>();

        for (int i = 0; i < iconsPosition.Count - 1; i++)
        {
            positionDiff.Add(new Vector2());
            positionDiff[i] = new Vector2(startPos.localPosition.x - iconsPosition[i].x, startPos.localPosition.y - iconsPosition[i].y);
        }

        for (int i = 0; i < iconsPosition.Count - 1; i++)
        {
            inventoryIconsImage[i].GetComponent<RectTransform>().localPosition = startPos.localPosition;
        }
    }

    private void Update()
    {
        if (!isAnimationOver)
        {
            if (isInventoryDisplayed)
            {
                for (int i = 0; i < inventoryIconsInteractable.Count - 1; i++)
                {
                    inventoryIconsInteractable[i].interactable = false;
                }

                InventoryHide();
            }

            if (!isInventoryDisplayed)
            {
                InventoryAppears();
            }
        }
    }

    public void ResetTimer()
    {
        apparitionCurrentTime = 0;
    }

    /// <summary>
    /// Affiche l'inventaire
    /// </summary>
    private void InventoryAppears()
    {
        if(apparitionCurrentTime < apparitionTimeMax)
        {
            apparitionCurrentTime += Time.deltaTime;
        }
        else
        {
            isAnimationOver = true;
            isInventoryDisplayed = true;
        }

        apparitionPercent = apparitionCurve.Evaluate(apparitionCurrentTime);

        iconsPosition[0] = new Vector2(iconsPosition[0].x + positionDiff[0].x * apparitionPercent, iconsPosition[0].y + positionDiff[0].y * apparitionPercent);
        iconsPosition[1] = new Vector2(iconsPosition[1].x + positionDiff[1].x * apparitionPercent, iconsPosition[1].y + positionDiff[1].y * apparitionPercent);
        iconsPosition[2] = new Vector2(iconsPosition[2].x + positionDiff[2].x * apparitionPercent, iconsPosition[2].y + positionDiff[2].y * apparitionPercent);
        iconsPosition[3] = new Vector2(iconsPosition[3].x + positionDiff[3].x * apparitionPercent, iconsPosition[3].y + positionDiff[3].y * apparitionPercent);
    }

    /// <summary>
    /// Cache l'inventaire
    /// </summary>
    private void InventoryHide()
    {
        if (apparitionCurrentTime < apparitionTimeMax)
        {
            apparitionCurrentTime += Time.deltaTime;
        }
        else
        {
            isAnimationOver = true;
            isInventoryDisplayed = false;
        }

        apparitionPercent = apparitionCurve.Evaluate(apparitionCurrentTime);

        iconsPosition[0] = new Vector2(iconsPosition[0].x - positionDiff[0].x * apparitionPercent, iconsPosition[0].y - positionDiff[0].y * apparitionPercent);
        iconsPosition[1] = new Vector2(iconsPosition[1].x - positionDiff[1].x * apparitionPercent, iconsPosition[1].y - positionDiff[1].y * apparitionPercent);
        iconsPosition[2] = new Vector2(iconsPosition[2].x - positionDiff[2].x * apparitionPercent, iconsPosition[2].y - positionDiff[2].y * apparitionPercent);
        iconsPosition[3] = new Vector2(iconsPosition[3].x - positionDiff[3].x * apparitionPercent, iconsPosition[3].y - positionDiff[3].y * apparitionPercent);
    }

    public void CollectObject()
    {

    }

    public void DropAnObject()
    {

    }

    public void AssignDescription()
    {

    }

    public void ShowDescription()
    {

    }
}
