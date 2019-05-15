using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySystem : MonoBehaviour
{
    [Header("Inventory References")]
    public Image inventoryInputImage;
    public Button inventoryButton;
    public RectTransform startPos;

    [Header("Inventory Conditions")]
    public bool isInventoryDisplayed = false;
    public bool isAnimationOver = true;
    public bool isThereAnyObjectInInventory;
    public bool canBeDisplayed = true;

    [Header("Inventory Sprites Icon")]
    public Sprite emptyInventorySprite;
    public Sprite fullInventorySprite;

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

    [Header("Animation to Add an Object")]
    public Animator addObject;



    public static InventorySystem Instance;





    private void Awake()
    {
        Instance = this;

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

        if (!isThereAnyObjectInInventory)
        {
            inventoryInputImage.sprite = emptyInventorySprite;
            inventoryInputImage.color = new Color(inventoryInputImage.color.r, inventoryInputImage.color.g, inventoryInputImage.color.b, 0.25f);
            inventoryButton.interactable = false;
        }
        else
        {
            inventoryInputImage.sprite = fullInventorySprite;
            inventoryInputImage.color = new Color(inventoryInputImage.color.r, inventoryInputImage.color.g, inventoryInputImage.color.b, 1);
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
            if (canBeDisplayed)
                InventoryAppears();
        }

        if (isThereAnyObjectInInventory)
        {
            inventoryButton.interactable = true;
            inventoryInputImage.color = new Color(inventoryInputImage.color.r, inventoryInputImage.color.g, inventoryInputImage.color.b, 1);

            if (isInventoryDisplayed)
            {
                inventoryInputImage.sprite = emptyInventorySprite;
            }
            else
            {
                inventoryInputImage.sprite = fullInventorySprite;
            }
        }
        else
        {
            inventoryButton.interactable = false;
            inventoryInputImage.sprite = emptyInventorySprite;
            inventoryInputImage.color = new Color(inventoryInputImage.color.r, inventoryInputImage.color.g, inventoryInputImage.color.b, 0.25f);
        }

    }

    /// <summary>
    /// Function to call to un/fold the inventory
    /// </summary>
    public void InventoryAnimation()
    {
        if (GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
        {
            CheckIcons();

            if (isThereAnyObjectInInventory)
            {
                isInventoryDisplayed = !isInventoryDisplayed;
                isAnimationOver = false;
                apparitionCurrentTime = 0;
            }
        }
    }

    public void HideInventory()
    {
        CheckIcons();

        if (isInventoryDisplayed)
        {
            isInventoryDisplayed = false;
            isAnimationOver = false;
            apparitionCurrentTime = 0;
        }
    }

    private void CheckIcons()
    {
        if (isInventoryDisplayed)
        {
            for (int i = 0; i < slotsBehaviour.Count; i++)
            {
                if (slotsBehaviour[i].isIconImageAppeared == false)
                {
                    slotsBehaviour[i].ShowIcon();
                }
            }
        }
        else
        {
            for (int i = 0; i < slotsBehaviour.Count; i++)
            {
                if (slotsBehaviour[i].isIconImageAppeared)
                {
                    slotsBehaviour[i].HideIcon();
                }
            }
        }
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
            isAnimationOver = true;
        }


        apparitionPercent = inventoryApparitionCurve.Evaluate(apparitionCurrentTime / apparitionTimeMax);

        slotsRectTransform[0].localPosition = new Vector2(iconsPosition[0].x + (positionDiff[0].x * apparitionPercent), iconsPosition[0].y + (positionDiff[0].y * apparitionPercent));
        slotsRectTransform[1].localPosition = new Vector2(iconsPosition[1].x + (positionDiff[1].x * apparitionPercent), iconsPosition[1].y + (positionDiff[1].y * apparitionPercent));
        slotsRectTransform[2].localPosition = new Vector2(iconsPosition[2].x + (positionDiff[2].x * apparitionPercent), iconsPosition[2].y + (positionDiff[2].y * apparitionPercent));
    }

    /// <summary>
    /// Ajoute un objet dans l'inventaire et assigne les 
    /// </summary>
    /// <param name="collectedObject"></param>
    public void AssignToAvailableSlot(Objet_Interaction collectedObject)
    {
        bool hasBeenAssigned = false;

        for (int i = 0; i < slotsBehaviour.Count; i++)
        {
            if (slotsBehaviour[i].isAssigned == false && hasBeenAssigned == false)
            {
                slotsBehaviour[i].uiObjectImage.sprite = collectedObject.objectImage;
                slotsBehaviour[i].uiObjectName.text = collectedObject.objectName;
                slotsBehaviour[i].uiObjectDescritpion.text = collectedObject.objectDescription;
                slotsBehaviour[i].isAssigned = true;
                hasBeenAssigned = true;
            }

            isThereAnyObjectInInventory = true;
            inventoryInputImage.sprite = fullInventorySprite;
            inventoryButton.interactable = true;

            inventoryInputImage.color = new Color(inventoryInputImage.color.r, inventoryInputImage.color.g, inventoryInputImage.color.b, 0.25f);
        }

        addObject.gameObject.GetComponent<Image>().sprite = collectedObject.objectImage;

        addObject.SetTrigger("Get");
    }

    /// <summary>
    /// Check si au moins 1 objet se trouve dans l'inventaire
    /// </summary>
    private void CheckInventory()
    {
        for (int i = 0; i < slotsBehaviour.Count; i++)
        {
            bool atLeastOne = false;

            if (slotsBehaviour[i].isAssigned == true)
            {
                atLeastOne = true;
            }

            if (atLeastOne)
            {
                isThereAnyObjectInInventory = true;
                inventoryInputImage.sprite = fullInventorySprite;
                inventoryButton.interactable = true;

                inventoryInputImage.color = new Color(inventoryInputImage.color.r, inventoryInputImage.color.g, inventoryInputImage.color.b, 1f);
            }
            else
            {
                isThereAnyObjectInInventory = false;
                inventoryInputImage.sprite = emptyInventorySprite;
                inventoryButton.interactable = false;

                inventoryInputImage.color = new Color(inventoryInputImage.color.r, inventoryInputImage.color.g, inventoryInputImage.color.b, 0.25f);
            }

        }
    }

    /// <summary>
    /// Retire un objet de l'inventaire
    /// </summary>
    public void DropAnObject(string objectNameToDrop)
    {
        for (int i = 0; i < slotsBehaviour.Count; i++)
        {
            if (slotsBehaviour[i].isAssigned == true)
            {
                if (slotsBehaviour[i].uiObjectName.text == objectNameToDrop)
                {
                    slotsBehaviour[i].isAssigned = false;
                    slotsBehaviour[i].uiObjectDescritpion = null;
                    slotsBehaviour[i].uiObjectImage = null;
                    slotsBehaviour[i].uiObjectName = null;
                }
            }
        }

        CheckInventory();
    }
}