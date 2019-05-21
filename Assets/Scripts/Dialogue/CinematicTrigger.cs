﻿using System.Collections;
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

    /// <summary>
    /// Lance la coroutine qui lance le dialogue
    /// </summary>
    public void LaunchCinematic()
    {
        StartCoroutine(DelayBeforeCinematic(1.5f));
    }

    /// <summary>
    /// Enclenche le mode dialogue
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Passe en mode Puzzle après un délai
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator DelayBoforeEndingCinematic(float time)
    {
        yield return new WaitForSeconds(time);

        EndCinematicNOW();
    }

    /// <summary>
    /// Passe en mode Puzzle
    /// </summary>
    public void EndCinematicNOW()
    {
        ROOM_Manager.Instance.currentRoom.DisableUI();

        Camera_UI.Instance.SwitchToNoUi();

        GameManager.Instance.SwitchModeTo(GameManager.GameMode.PuzzleMode);
    }
}