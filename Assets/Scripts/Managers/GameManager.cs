using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameMode { PuzzleMode, InvestigationMode, InteractingMode, CinematicMode, MenuMode }

    [Header("Mode de jeu")]
    public GameMode currentGameMode;
    public GameMode previousGameMode;

    [Header("Niveau")]
    public int currentLevel;
    public int progressionLevel;


    public static GameManager Instance;



    private void Awake()
    {
        Instance = this;
    }


    public void SwitchModeTo(GameMode nextMode)
    {
        currentGameMode = nextMode;

        switch(currentGameMode)
        {
            case GameMode.PuzzleMode:
                UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.inventoryGO);

                UI_Manager.Instance.outOfContextGO[0].GetComponent<Button>().interactable = false;

                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.contextuelleGO));
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.backgroundGO));

                previousGameMode = GameMode.PuzzleMode;
                break;

            case GameMode.InvestigationMode:
                if (previousGameMode == GameMode.PuzzleMode)
                {
                    UI_Manager.Instance.inventoryGO[2].GetComponent<Button>().onClick.Invoke();

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.contextuelleGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.backgroundGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.inventoryGO));

                    previousGameMode = GameMode.InvestigationMode;
                }
                else
                {


                previousGameMode = GameMode.InvestigationMode;
                }
                break;

            case GameMode.InteractingMode:


                previousGameMode = GameMode.InteractingMode;
                break;
        }
    }


    /// <summary>
    /// Set values
    /// </summary>
    private void Start()
    {
        //DataManager.Instance.Load();
    }

    public void SaveProgression()
    {
        //DataManager.Instance.Save();
    }
}
