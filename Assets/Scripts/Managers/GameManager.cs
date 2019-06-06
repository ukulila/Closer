using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameMode { PuzzleMode, InvestigationMode, Dialogue, CinematicMode, ClueMode, MenuMode }

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
                    UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.WinningGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.dialogueGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.backgroundGO));
                }

                if (previousGameMode == GameMode.InvestigationMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.inventoryGO);


                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.contextuelleGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.backgroundGO));
                }

                if (previousGameMode == GameMode.CinematicMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.inventoryGO);
                    UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.WinningGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.dialogueGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.contextuelleGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.backgroundGO));
                }

                if (ROOM_Manager.Instance.currentRoom.isThereClients == true)
                {
                    Harcelement_Manager.Instance.AmongThem();
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

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.inventoryGO));
                }

                if (previousGameMode == GameMode.Dialogue)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.contextuelleGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.dialogueGO));
                }

                Harcelement_Manager.Instance.FarFromThem();

                //playerBehav.enabled = false;

                previousGameMode = GameMode.InvestigationMode;
                break;

            case GameMode.Dialogue:
                if (previousGameMode == GameMode.PuzzleMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.dialogueGO);
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.backgroundGO);
                    UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.contextuelleGO);

                    UI_Manager.Instance.inventoryGO[2].GetComponent<Button>().onClick.Invoke();
                    UI_Manager.Instance.inventoryButtonsGO[0].GetComponentInChildren<Button>().onClick.Invoke();
                    InventorySystem.Instance.HideInventory();

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.inventoryGO));
                }

                if (previousGameMode == GameMode.InvestigationMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.dialogueGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.contextuelleGO));
                }

                //playerBehav.enabled = false;

                Harcelement_Manager.Instance.FarFromThem();

                previousGameMode = GameMode.Dialogue;
                break;

            case GameMode.CinematicMode:
                if (previousGameMode == GameMode.PuzzleMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.dialogueGO);
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.backgroundGO);
                    UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.contextuelleGO);

                    UI_Manager.Instance.inventoryGO[2].GetComponent<Button>().onClick.Invoke();
                    UI_Manager.Instance.inventoryButtonsGO[0].GetComponentInChildren<Button>().onClick.Invoke();
                    InventorySystem.Instance.HideInventory();

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.inventoryGO));
                }

                if (previousGameMode == GameMode.InvestigationMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.dialogueGO);

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.contextuelleGO));
                }
                previousGameMode = GameMode.CinematicMode;
                break;

            case GameMode.ClueMode:

                if (previousGameMode == GameMode.PuzzleMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.WinningGO);

                    UI_Manager.Instance.inventoryGO[2].GetComponent<Button>().onClick.Invoke();
                    UI_Manager.Instance.inventoryButtonsGO[0].GetComponentInChildren<Button>().onClick.Invoke();
                    InventorySystem.Instance.HideInventory();

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.inventoryGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.inventoryButtonsGO));
                }

                if (previousGameMode == GameMode.InvestigationMode)
                {
                    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.WinningGO);

                    Camera_UI.Instance.SwitchToNoUi();

                    ROOM_Manager.Instance.currentRoom.DisableUI();

                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.contextuelleGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.backgroundGO));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.DelayBeforeDeactivation(1.5f, UI_Manager.Instance.inventoryButtonsGO));
                }

                Objectif_Scr.Instance.Disappearance();

                Harcelement_Manager.Instance.FarFromThem();

                previousGameMode = GameMode.ClueMode;
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
