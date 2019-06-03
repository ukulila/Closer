using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnim : MonoBehaviour
{

    public Animator UILevelSelectionAnimator;

    public Animator OptionAnimator;

    public bool SoundIn;
    public bool MusicIn;
    public bool CreditsIn;


    public void Set_myAnimation()
    {

        UILevelSelectionAnimator.SetTrigger("In");

    }

    public void SetSound()
    {
        if (!SoundIn)
        {
            OptionAnimator.SetTrigger("SoundOn");
            SoundIn = true;
            return;
        }

        if (SoundIn)
        {
            OptionAnimator.SetTrigger("SoundOff");
            SoundIn = false;
            return;
        }
    }

    public void SetMusic()
    {
        if (!MusicIn)
        {
            OptionAnimator.SetTrigger("MusicOn");
            MusicIn = true;
            return;
        }

        if (MusicIn)
        {
            OptionAnimator.SetTrigger("MusicOff");
            MusicIn = false;
            return;
        }
    }


    public void SetCredits()
    {
        if (!CreditsIn)
        {
            OptionAnimator.SetTrigger("CreditsOn");
            CreditsIn = true;
            return;
        }

        if (CreditsIn)
        {
            OptionAnimator.SetTrigger("CreditsOff");
            CreditsIn = false;
            return;
        }
    }

    public void ResetAllOptions()
    {
        CreditsIn = false;
        SoundIn = false;
        MusicIn = false;

    }



}
