using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameMode { PuzzleMode, InvestigationMode, Dialogue, CinematicMode, MenuMode }

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

        switch (currentGameMode)
        {
            case GameMode.PuzzleMode:
                if (previousGameMode == GameMode.Dialogue)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.inventoryGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.dialogueGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.backgroundGO));
                }
                else
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.inventoryGO);

                    UI_Manager.Instance.outOfContextGO[0].GetComponent<Button>().interactable = false;

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.contextuelleGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.backgroundGO));
                }
                previousGameMode = GameMode.PuzzleMode;
                break;

            case GameMode.InvestigationMode:
                if (previousGameMode == GameMode.PuzzleMode)
                {
                    UI_Manager.Instance.inventoryGO[2].GetComponent<Button>().onClick.Invoke();
                    UI_Manager.Instance.inventoryButtonsGO[0].GetComponentInChildren<Button>().onClick.Invoke();
                    InventorySystem.Instance.HideInventory();

                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.backgroundGO);
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.contextuelleGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2f, UI_Manager.Instance.inventoryGO));
                }
                else
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.contextuelleGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.dialogueGO));
                }

                previousGameMode = GameMode.InvestigationMode;
                break;

            case GameMode.Dialogue:
                if (previousGameMode == GameMode.PuzzleMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.dialogueGO);
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.backgroundGO);

                    UI_Manager.Instance.inventoryGO[2].GetComponent<Button>().onClick.Invoke();
                    UI_Manager.Instance.inventoryButtonsGO[0].GetComponentInChildren<Button>().onClick.Invoke();
                    InventorySystem.Instance.HideInventory();

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.inventoryGO));
                }
                else
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.dialogueGO);

                    UI_Manager.Instance.outOfContextGO[0].GetComponent<Button>().interactable = false;
                    UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.outOfContextGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(2, UI_Manager.Instance.contextuelleGO));
                }

                previousGameMode = GameMode.Dialogue;
                break;
        }
    }


    /// <summary>
    /// Set values
    /// </summary>
    private void Start()
    {
        DataManager.Instance.Load();

        SaveProgression();
    }

    public void SaveProgression()
    {
        DataManager.Instance.Save();
    }
}
