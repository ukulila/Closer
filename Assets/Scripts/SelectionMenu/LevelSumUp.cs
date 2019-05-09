using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSumUp : MonoBehaviour
{
    public SmoothMoveSwipe smoothRef;

    public Sprite spriteClue;
    public Sprite spriteUnknown;

    public string levelName;
    public string levelDescription;
    public string clueName;
    public string clueDescription;

    public bool isLevelFinished = false;

    public Animator lvlsAnim;

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

    public void Unlocked()
    {
        lvlsAnim.SetTrigger("Enabled");
        lvlsAnim.SetTrigger("Selected");

    }
}
