using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_Behaviour : MonoBehaviour
{
    [Header("Description Parameters & Co")]
    public bool isAssigned = false;

    [Space]

    [Header("   Icon")]
    public Image uiObjectImage;
    public Button iconButton;
    [Header("   Background")]
    public Button backgroundButton;
    public Image backgroundImage;
    public RectTransform backgroundSize;
    [Header("   Text")]
    public TextMeshProUGUI uiObjectDescritpion;
    public TextMeshProUGUI uiObjectName;

    [Header("Icon Animation")]
    public AnimationCurve iconImageApparition;
    public float iconImageApparitionPercent;
    public float iconImageApparitionTimeMax;
    public float iconImageApparitionCurrentTime;
    public bool isIconImageAnimationOver = false;
    public bool isIconImageAppeared = false;

    [Header("Name Animation")]
    public AnimationCurve nameApparition;
    public float nameApparitionPercent;
    public float nameApparitionTimeMax;
    public float nameApparitionCurrentTime;
    public bool isNameAnimationOver = false;
    public bool isNameAppeared = false;

    [Header("Description Animation")]
    public AnimationCurve descriptionApparition;
    public float descriptionApparitionPercent;
    public float descriptionApparitionTimeMax;
    public float descriptionApparitionCurrentTime;
    public bool isDescriptionAnimationOver = false;
    public bool isDescriptionAppeared = false;

    [Header("Position for animation")]
    public Vector2 descriptionPos;
    public Vector2 defaultPos;
    public Vector2 posDiff;

    [Header("Size for animation")]
    public Vector2 currentBackgroudSize;

    public Vector2 defaultBackgroudSize;
    public Vector2 nameBackgroudSize;
    public Vector2 descriptionBackgroudSize;
    public Vector2 sizeDiff;




    private void Update()
    {
        if (!isIconImageAnimationOver && isIconImageAppeared && isDescriptionAnimationOver && !isDescriptionAppeared && isNameAnimationOver && !isNameAppeared)
        {
            ShowIconAnimation();
        }

        if (!isIconImageAnimationOver && !isIconImageAppeared && isDescriptionAnimationOver && !isDescriptionAppeared && isNameAnimationOver && !isNameAppeared)
        {
            HideIconAnimation();
        }


        if (isAssigned)
        {
            currentBackgroudSize = backgroundSize.sizeDelta;

            if (!isDescriptionAnimationOver && isDescriptionAppeared && isNameAnimationOver && isNameAppeared && isIconImageAnimationOver && isIconImageAppeared)
            {
                ShowDescriptionAnimation();
            }

            if (!isDescriptionAnimationOver && !isDescriptionAppeared && isNameAnimationOver && isNameAppeared && isIconImageAnimationOver && isIconImageAppeared)
            {
                HideDescriptionAnimation();
            }

            if (!isNameAnimationOver && isNameAppeared && isDescriptionAnimationOver && !isDescriptionAppeared && isIconImageAnimationOver && isIconImageAppeared)
            {
                ShowNameAnimation();
            }

            if (!isNameAnimationOver && !isNameAppeared && isDescriptionAnimationOver && !isDescriptionAppeared && isIconImageAnimationOver && isIconImageAppeared)
            {
                HideNameAnimation();
            }
        }
    }

    #region Functions to call
    public void InverseName()
    {
        isNameAnimationOver = false;
        isNameAppeared = !isNameAppeared;
        nameApparitionCurrentTime = 0;
    }

    public void ShowName()
    {
        isNameAnimationOver = false;
        isNameAppeared = true;
        nameApparitionCurrentTime = 0;
    }

    public void HideName()
    {
        isNameAnimationOver = false;
        isNameAppeared = false;
        nameApparitionCurrentTime = 0;
    }


    public void InverseDescription()
    {
        isDescriptionAnimationOver = false;
        isDescriptionAppeared = !isDescriptionAppeared;
        descriptionApparitionCurrentTime = 0;
    }

    public void ShowDescription()
    {
        isDescriptionAnimationOver = false;
        isDescriptionAppeared = true;
        descriptionApparitionCurrentTime = 0;
    }

    public void HideDescription()
    {
        isDescriptionAnimationOver = false;
        isDescriptionAppeared = false;
        descriptionApparitionCurrentTime = 0;
    }


    public void InverseIcon()
    {
        isIconImageAnimationOver = false;
        isIconImageAppeared = !isIconImageAppeared;
        iconImageApparitionCurrentTime = 0;
    }

    public void ShowIcon()
    {
        isIconImageAnimationOver = false;
        isIconImageAppeared = true;
        iconImageApparitionCurrentTime = 0;
    }

    public void HideIcon()
    {
        isIconImageAnimationOver = false;
        isIconImageAppeared = false;
        iconImageApparitionCurrentTime = 0;
    }
    #endregion

    #region Animation Curves
    /// <summary>
    /// Affiche l'icone
    /// </summary>
    public void ShowIconAnimation()
    {
        if (iconImageApparitionCurrentTime < iconImageApparitionTimeMax)
        {
            iconImageApparitionCurrentTime += Time.deltaTime;
            iconButton.interactable = false;
        }
        else
        {
            isIconImageAnimationOver = true;
        }

        iconImageApparitionPercent = nameApparition.Evaluate(iconImageApparitionCurrentTime / iconImageApparitionTimeMax);

        if (isAssigned)
            uiObjectImage.color = new Color(uiObjectImage.color.r, uiObjectImage.color.g, uiObjectImage.color.b, 0 + 1 * iconImageApparitionPercent);

        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0 + 0.58f * iconImageApparitionPercent);
    }

    /// <summary>
    /// Cache l'icone
    /// </summary>
    public void HideIconAnimation()
    {
        if (iconImageApparitionCurrentTime < iconImageApparitionTimeMax)
        {
            iconImageApparitionCurrentTime += Time.deltaTime;
            iconButton.interactable = false;
        }
        else
        {
            isIconImageAnimationOver = true;
        }

        iconImageApparitionPercent = nameApparition.Evaluate(iconImageApparitionCurrentTime / iconImageApparitionTimeMax);
        if (isAssigned)
            uiObjectImage.color = new Color(uiObjectImage.color.r, uiObjectImage.color.g, uiObjectImage.color.b, 1 - 1 * iconImageApparitionPercent);

        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0.58f - 0.58f * iconImageApparitionPercent);
    }

    /// <summary>
    /// Affiche le nom de l'objet
    /// </summary>
    public void ShowNameAnimation()
    {
        if (nameApparitionCurrentTime < nameApparitionTimeMax)
        {
            nameApparitionCurrentTime += Time.deltaTime;
            iconButton.interactable = false;
        }
        else
        {
            isNameAnimationOver = true;
        }

        nameApparitionPercent = nameApparition.Evaluate(nameApparitionCurrentTime / nameApparitionTimeMax);

        backgroundSize.sizeDelta = new Vector2(defaultBackgroudSize.x + ((nameBackgroudSize.x - defaultBackgroudSize.x) * nameApparitionPercent), defaultBackgroudSize.y);
    }

    /// <summary>
    /// Cache le nom de l'objet
    /// </summary>
    public void HideNameAnimation()
    {
        if (nameApparitionCurrentTime < nameApparitionTimeMax)
        {
            nameApparitionCurrentTime += Time.deltaTime;
            iconButton.interactable = false;
        }
        else
        {
            isNameAnimationOver = true;
        }

        nameApparitionPercent = nameApparition.Evaluate(nameApparitionCurrentTime / nameApparitionTimeMax);

        backgroundSize.sizeDelta = new Vector2(defaultBackgroudSize.x - ((nameBackgroudSize.x - defaultBackgroudSize.x) * nameApparitionPercent), defaultBackgroudSize.y);
    }

    /// <summary>
    /// Affiche la description de l'objet
    /// </summary>
    public void ShowDescriptionAnimation()
    {
        if (descriptionApparitionCurrentTime < descriptionApparitionTimeMax)
        {
            descriptionApparitionCurrentTime += Time.deltaTime;
            backgroundButton.interactable = false;
        }
        else
        {
            isDescriptionAnimationOver = true;
        }

        descriptionApparitionPercent = nameApparition.Evaluate(descriptionApparitionCurrentTime / descriptionApparitionTimeMax);

        backgroundSize.sizeDelta = new Vector2(nameBackgroudSize.x, nameBackgroudSize.y + ((descriptionBackgroudSize.y - nameBackgroudSize.y) * descriptionApparitionPercent));
    }

    /// <summary>
    /// Cache la description de l'objet
    /// </summary>
    public void HideDescriptionAnimation()
    {
        if (descriptionApparitionCurrentTime < descriptionApparitionTimeMax)
        {
            descriptionApparitionCurrentTime += Time.deltaTime;
            backgroundButton.interactable = false;
        }
        else
        {
            isDescriptionAnimationOver = true;
        }

        descriptionApparitionPercent = nameApparition.Evaluate(descriptionApparitionCurrentTime / descriptionApparitionTimeMax);

        backgroundSize.sizeDelta = new Vector2(nameBackgroudSize.x, nameBackgroudSize.y - ((descriptionBackgroudSize.y - nameBackgroudSize.y) * descriptionApparitionPercent));
    }
    #endregion

}
