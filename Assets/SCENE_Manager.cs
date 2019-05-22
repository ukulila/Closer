using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCENE_Manager : MonoBehaviour
{
    public int currentSceneIndex;

    public int currentNextSceneToLoad;
    public int numberOfScene;

    public static SCENE_Manager Instance;



    private void Awake()
    {
        Instance = this;

        numberOfScene = SceneManager.sceneCountInBuildSettings;

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }


    public void SetNextSceneToLoad(int nextSceneIndex)
    {
        currentNextSceneToLoad = nextSceneIndex;
    }


    public void Loading()
    {
        SceneManager.LoadScene(currentNextSceneToLoad, LoadSceneMode.Single);
    }
}
