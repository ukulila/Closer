using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    public List<LevelSumUp> levels;
    public int progressionIndex;

    public static LevelManager Instance;



    private void Awake()
    {
        Instance = this;
    }

    public void SetUpProgression()
    {
        for (int i = 0; i < progressionIndex; i++)
        {
            levels[i].lockedImage.color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < progressionIndex - 1; i++)
        {
            levels[i].isLevelFinished = true;
        }

        levels[progressionIndex].lvlAnim.SetTrigger("Reveal");
    }
}
