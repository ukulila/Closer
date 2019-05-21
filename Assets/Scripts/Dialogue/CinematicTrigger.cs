using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    public bool playOnAwake;

    public NPCInteractions dialogueNpcToActivate;
    public string NpcLocation;


    public static CinematicTrigger Instance;




    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        if (playOnAwake)
        {
            StartCoroutine(DelayBeforeCinematic(0.1f));
        }
    }


    public void LaunchCinematic()
    {
        StartCoroutine(DelayBeforeCinematic(1.5f));
    }


    IEnumerator DelayBeforeCinematic(float time)
    {
        yield return new WaitForSeconds(0.3f);

        GameManager.Instance.SwitchModeTo(GameManager.GameMode.Dialogue);

        NpcLocation = dialogueNpcToActivate.transform.parent.name;

        Camera_UI.Instance.SetInitialCameraValues();
        Camera_UI.Instance.SetCameraNextLocation(NpcLocation);

        yield return new WaitForSeconds(0.1f);

        Camera_UI.Instance.switchToUI = true;
        Camera_UI.Instance.cameraReposition = false;

        yield return new WaitForSeconds(time);

        StartCoroutine(ROOM_Manager.Instance.ActiveAnimationIn(0.1f));

        yield return new WaitForSeconds(1.5f);

        dialogueNpcToActivate.StartDialogueAbout();
    }


    public void EndCinematic()
    {
        ROOM_Manager.Instance.currentRoom.DisableUI();

        Camera_UI.Instance.SwitchToNoUi();

        GameManager.Instance.SwitchModeTo(GameManager.GameMode.PuzzleMode);
    }
}
