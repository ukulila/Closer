using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ROOM_Manager : MonoBehaviour
{
    public RoomInteraction currentRoom;


    public static ROOM_Manager Instance;




    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Invoque l'UI selon les presets de la Room
    /// </summary>
    public void LaunchUI(float animDelay)
    {
        if (currentRoom != null)
        {
            StartCoroutine(ActiveAnimationIn(animDelay));

            GameManager.Instance.SwitchModeTo(GameManager.GameMode.InvestigationMode);
        }
    }

    /// <summary>
    /// Enleve l'UI
    /// </summary>
    public void DeactivateUI()
    {
        if (currentRoom != null)
        {
            GameManager.Instance.SwitchModeTo(GameManager.GameMode.PuzzleMode);

            currentRoom.DisableUI();
        }
    }

    public void ExamineRoom()
    {
        Camera_UI.Instance.SetContextuelleRepositionValues(currentRoom.gameObject.name);

        Camera_UI.Instance.SwitchToUI();
    }


    public IEnumerator ActiveAnimationIn(float time)
    {
        yield return new WaitForSeconds(time);

        if (currentRoom.isDialogue)
            NPC_Manager.Instance.SetDialogueOPTION();

        currentRoom.InteractionAppears();

        yield return new WaitForSeconds(2f);

        Examine_Script.Instance.examineButton.interactable = true;
    }

    public IEnumerator DeactiveAnimationIn(float time)
    {
        yield return new WaitForSeconds(time);

        currentRoom.DisableUI();
    }
}
