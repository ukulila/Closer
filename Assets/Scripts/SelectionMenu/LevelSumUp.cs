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

    public Animator[] lvlsAnim;

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

    public void ChosenLevel1()
    {
        lvlsAnim[0].SetBool("Open1", true);
        lvlsAnim[1].SetBool("Closed2", true);
        lvlsAnim[2].SetBool("Closed3", true);
        lvlsAnim[3].SetBool("Closed4", true);
    }

    public void ChosenLevel2()
    {
        lvlsAnim[1].SetBool("Open2", true);
        lvlsAnim[0].SetBool("Closed1", true);
        lvlsAnim[2].SetBool("Closed3", true);
        lvlsAnim[3].SetBool("Closed4", true);
    }

    public void ChosenLevel3()
    {
        lvlsAnim[2].SetBool("Open3", true);
        lvlsAnim[1].SetBool("Closed2", false);
        lvlsAnim[0].SetBool("Closed1", false);
        lvlsAnim[3].SetBool("Closed4", false);
    }

    public void ChosenLevel4()
    {
        lvlsAnim[3].SetBool("Open4", true);
        lvlsAnim[1].SetBool("Closed2", true);
        lvlsAnim[2].SetBool("Closed3", true);
        lvlsAnim[0].SetBool("Closed1", true);
    }
}
