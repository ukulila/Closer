﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue_Script : MonoBehaviour
{
    public Animator clueAnim;


    public static Clue_Script Instance;



    public void LaunchClue()
    {
        StartCoroutine(DelayBeforeClue());
    }

    public void GoBackToMenu()
    {
        StartCoroutine(DelayBeforeGoingBack());
    }


    IEnumerator DelayBeforeClue()
    {
        GameManager.Instance.SwitchModeTo(GameManager.GameMode.ClueMode);

        yield return new WaitForSeconds(2f);

        clueAnim.SetTrigger("FadeIn");
    }

    IEnumerator DelayBeforeGoingBack()
    {
        yield return new WaitForSeconds(2f);

    }
}
