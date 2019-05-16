using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ROOM_Manager : MonoBehaviour
{
    public RoomInteraction currentRoom;
    public Button outOfContext;


    public static ROOM_Manager Instance;




    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Invoque l'UI selon les presets de la Room
    /// </summary>
    public void LaunchUI()
    {
        if (currentRoom != null)
        {
            currentRoom.InteractionAppears();
            StartCoroutine(ActiveIn());
            GameManager.Instance.currentGameMode = GameManager.GameMode.InvestigationMode;
        }
    }

    ///// <summary>
    ///// Active les bons GO
    ///// </summary>
    //public void Active()
    //{
    //    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.backgroundGO);
    //    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.contextuelleGO);
    //    UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.inventoryButtonsGO);

    //    UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.dialogueGO);
    //    UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.inventoryGO);
    //}

    ///// <summary>
    ///// Desactive les bons GO
    ///// </summary>
    //public void Desactive()
    //{
    //    StartCoroutine(DesactiveIn());

    //    //UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.dialogueGO);
    //    //UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.inventoryGO);
    //}

    /// <summary>
    /// Enleve l'UI
    /// </summary>
    public void DeactivateUI()
    {
        if (currentRoom != null)
        {
            currentRoom.DisableUI();
            outOfContext.interactable = false;
            GameManager.Instance.currentGameMode = GameManager.GameMode.PuzzleMode;
        }

    }

    IEnumerator DesactiveIn()
    {
        yield return new WaitForSeconds(2);

        

        //UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.backgroundGO);
        //UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.contextuelleGO);
    }

    IEnumerator ActiveIn()
    {
        yield return new WaitForSeconds(2);

        outOfContext.interactable = true;
    }
}
