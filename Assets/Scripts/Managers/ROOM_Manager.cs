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
            UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.outOfContextGO);

            StartCoroutine(ActiveOutContext());
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
            UI_Manager.Instance.outOfContextGO[0].GetComponent<Button>().interactable = false;
            UI_Manager.Instance.DeactivateListOfUI(UI_Manager.Instance.outOfContextGO);

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

        currentRoom.InteractionAppears();
    }

    public IEnumerator DeactiveAnimationIn(float time)
    {
        yield return new WaitForSeconds(time);

        currentRoom.DisableUI();
    }

    IEnumerator ActiveOutContext()
    {
        yield return new WaitForSeconds(2);

        UI_Manager.Instance.outOfContextGO[0].GetComponent<Button>().interactable = true;
    }
}
