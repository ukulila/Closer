using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Progression dans les niveaux")]
    public List<LevelSumUp> levels;
    public int progressionIndex;

    [Header("Niveau en CloseUp")]
    public LevelSumUp levelOnCloseUp;
    public int closeUpLevelIndex = 0;

    [Header("Move Away Animation parameters")]
    public AnimationCurve moveAwayCurve;
    public float currentMoveTime;
    public float maxMoveTime;
    public float currentMovePercent;
    public float furtherInX;
    [Space]
    public bool isMoveAwayAnimationOver = true;
    public bool isMovedAway = false;
    [Space]
    public bool isCloseUpAllSet = true;

    [Header("   Records of Levels Initial Position")]
    public List<Vector2> posRef;

    [Header("Close Up Mode")]
    public Button playButton;
    public Button levels_ReturnToMain;
    public Button levels_ReturnToGlobal;

    public Animator closeUpAnim;




    public static LevelManager Instance;



    private void Awake()
    {
        Instance = this;

        SetUpProgression();

        for (int i = 0; i < levels.Count; i++)
        {
            posRef.Add(levels[i].GetComponent<RectTransform>().localPosition);
        }
    }


    private void Update()
    {
        if (!isMoveAwayAnimationOver && SmoothMoveSwipe.Instance.isCranAnimationOver == true)
        {
            MoveLevels();
        }

        if (isMoveAwayAnimationOver /*&& SmoothMoveSwipe.Instance.isCranAnimationOver == true*/ && isCloseUpAllSet)
        {
            //ShowCloseUp();
        }
    }

    /// <summary>
    /// Set Up la progression du joueur
    /// </summary>
    public void SetUpProgression()
    {
        for (int i = 0; i < progressionIndex; i++)
        {
            levels[i].lockedImage.color = new Color32((byte)255, (byte)255, (byte)255, (byte)0);
        }

        for (int i = progressionIndex; i < levels.Count; i++)
        {
            levels[i].GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < progressionIndex - 1; i++)
        {
            levels[i].isLevelFinished = true;
        }

        levels[progressionIndex].lvlAnim.SetTrigger("Reveal");
    }

    /// <summary>
    /// Impose le "crantage" au niveau de ce level
    /// </summary>
    public void CrantageToSelectedLevel()
    {
        SmoothMoveSwipe.Instance.isSwipping = false;

        isCloseUpAllSet = false;

        levels_ReturnToMain.gameObject.SetActive(false);
        StartCoroutine(GoCloseIn());

        currentMoveTime = 0;
        isMovedAway = true;
        isMoveAwayAnimationOver = false;

        SmoothMoveSwipe.Instance.lastPos = SmoothMoveSwipe.Instance.line.localPosition;

        for (int i = 0; i < SmoothMoveSwipe.Instance.levelCranRef.Count; i++)
        {
            Vector2 rightLimit = new Vector2(0, 0);
            Vector2 leftLimit = new Vector2(0, 0);

            Debug.Log("level selected" + levelOnCloseUp.gameObject.name);

            if (levelOnCloseUp.gameObject.name == levels[i].gameObject.name)
            {
                closeUpLevelIndex = i;

                SmoothMoveSwipe.Instance.diffPos = SmoothMoveSwipe.Instance.levelCranRef[i] - SmoothMoveSwipe.Instance.lastPos;

                SmoothMoveSwipe.Instance.isCranAnimationOver = false;

                SCENE_Manager.Instance.SetNextSceneToLoad(closeUpLevelIndex);
            }
        }

        isCloseUpAllSet = true;
    }

    /// <summary>
    /// Déplace en x la position des niveaux situés à DROITE de celui en Close Up
    /// </summary>
    public void MoveLevels()
    {
        if (currentMoveTime < maxMoveTime)
        {
            currentMoveTime += Time.deltaTime;
        }
        else
        {
            isMoveAwayAnimationOver = true;

            if (isMovedAway)
                ShowCloseUp();
        }

        currentMovePercent = moveAwayCurve.Evaluate(currentMoveTime / maxMoveTime);

        if (isMovedAway)
        {
            for (int i = closeUpLevelIndex + 1; i < levels.Count; i++)
            {
                levels[i].GetComponent<RectTransform>().localPosition = new Vector2((posRef[i].x + (furtherInX * currentMovePercent)), posRef[i].y);
            }
        }
        else
        {
            for (int i = closeUpLevelIndex + 1; i < levels.Count; i++)
            {
                levels[i].GetComponent<RectTransform>().localPosition = new Vector2(((posRef[i].x + furtherInX) - (furtherInX * currentMovePercent)), posRef[i].y);
            }
        }
    }

    public void ShowCloseUp()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].gameObject.GetComponent<Image>().raycastTarget = false;
            levels[i].gameObject.GetComponent<Button>().interactable = false;
        }

        playButton.interactable = true;
        playButton.gameObject.GetComponent<Image>().raycastTarget = true;
        playButton.gameObject.GetComponent<Animator>().SetTrigger("FadeIn");

        levelOnCloseUp.gameObject.GetComponent<Animator>().SetTrigger("FadeOut");

        closeUpAnim.SetTrigger("FadeIn");
    }

    /// <summary>
    /// Set les bonnes valeurs pour revenir en selection global des niveaux
    /// </summary>
    public void GoBackToGlobal()
    {
        levelOnCloseUp.gameObject.GetComponent<Animator>().SetTrigger("FadeIn");

        closeUpAnim.ResetTrigger("FadeIn");
        closeUpAnim.SetTrigger("FadeOut");

        currentMoveTime = 0;
        isMovedAway = false;
        isMoveAwayAnimationOver = false;

        playButton.interactable = false;
        playButton.gameObject.GetComponent<Image>().raycastTarget = false;
        playButton.gameObject.GetComponent<Animator>().SetTrigger("FadeOut");

        StartCoroutine(GoBackIn());
    }

    private IEnumerator GoBackIn()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < progressionIndex; i++)
        {
            levels[i].gameObject.GetComponent<Image>().raycastTarget = true;
            levels[i].gameObject.GetComponent<Button>().interactable = true;
        }

        levels_ReturnToMain.gameObject.SetActive(true);

        SmoothMoveSwipe.Instance.isSwipping = true;
    }

    private IEnumerator GoCloseIn()
    {
        yield return new WaitForSeconds(2f);

        levels_ReturnToGlobal.interactable = true;
    }
}
