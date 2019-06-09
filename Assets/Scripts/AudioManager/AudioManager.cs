using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ClickButton;
    public AudioSource TrueTimeline;
    public AudioSource FalseTimeline;
    public AudioSource PlayGameSound;
    public AudioSource DropClue;

    public static AudioManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void PlayClickSound()
    {
        ClickButton.Play();
    }

    public void PlayTrueTimeLineSound()
    {
        TrueTimeline.Play();
    }

    public void PlayFalseTimeLineSound()
    {
        FalseTimeline.Play();
    }

    public void PlayPlayGameSound()
    {
        PlayGameSound.Play();
    }

    public void PlayDropClueSound()
    {
        DropClue.Play();
    }

}
