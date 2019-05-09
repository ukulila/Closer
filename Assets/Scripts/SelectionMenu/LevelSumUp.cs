using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSumUp : MonoBehaviour
{
    public Sprite spriteClue;
    public Sprite spriteUnknown;
    public Image lockedImage;


    public string levelName;
    public string levelDescription;
    public string clueName;
    public string clueDescription;

    public bool isLevelFinished = false;

    public Animator lvlAnim;



    private void Awake()
    {
        //lockedImage = GameObject.Find("LockedImage").GetComponent<Image>();
    }


    public void AssignToCloseUp()
    {
        if (isLevelFinished)
        {
            LevelCloseUp.Instance.polaroideImage.sprite = spriteClue;
            LevelCloseUp.Instance.clueNameCU.text = clueName;
            LevelCloseUp.Instance.clueDescriptionCU.text = clueDescription;
        }
        else
        {
            LevelCloseUp.Instance.polaroideImage.sprite = spriteUnknown;
            LevelCloseUp.Instance.clueNameCU.text = "";
            LevelCloseUp.Instance.clueDescriptionCU.text = "";
        }

        LevelCloseUp.Instance.levelNameCU.text = levelName;
        LevelCloseUp.Instance.levelDescriptionCU.text = levelDescription;
    }

    public void CrantageToSelectedLevel()
    {
        SmoothMoveSwipe.Instance.isSwipping = false;

        SmoothMoveSwipe.Instance.lastPos = SmoothMoveSwipe.Instance.line.localPosition;

        for (int i = 0; i < SmoothMoveSwipe.Instance.levelCranRef.Count - 1; i++)
        {
            Vector2 rightLimit = new Vector2(0, 0);
            Vector2 leftLimit = new Vector2(0, 0);


            if (this.gameObject.name == LevelManager.Instance.levels[i].gameObject.name)
            {

                SmoothMoveSwipe.Instance.diffPos = SmoothMoveSwipe.Instance.levelCranRef[i] - SmoothMoveSwipe.Instance.lastPos;

                Debug.Log(SmoothMoveSwipe.Instance.diffPos);

                SmoothMoveSwipe.Instance.isCranAnimationOver = false;
                return;
            }
        }
    }

}
