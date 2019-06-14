using UnityEngine;
using UnityEngine.UI;

public class LevelSumUp : MonoBehaviour
{
    public Sprite spriteClue;
    public Sprite spriteUnknown;
    public Image lockedImage;
    public RectTransform pin;

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

    /// <summary>
    /// Assigne les informations du niveau au CloseUp à suivre
    /// </summary>
    public void AssignToCloseUp()
    {
        LevelManager.Instance.levelOnCloseUp = this;

        if (isLevelFinished)
        {
            LevelCloseUp.Instance.polaroideImage.sprite = spriteClue;
            LevelCloseUp.Instance.clueNameCU.text = clueName;
            LevelCloseUp.Instance.clueDescriptionCU.text = clueDescription;
        }
        else
        {
            LevelCloseUp.Instance.polaroideImage.sprite = spriteUnknown;
            LevelCloseUp.Instance.clueNameCU.text = " ";
            LevelCloseUp.Instance.clueDescriptionCU.text = " ";
        }
        
        LevelCloseUp.Instance.levelNameCU.text = levelName;
        LevelCloseUp.Instance.levelDescriptionCU.text = levelDescription;

    }
}
