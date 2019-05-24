using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimoOptionManager : MonoBehaviour
{

    public Animator animator;


    public void SousMenuSound()
    {
        animator.SetBool("SoundSelected", true);
    }

    public void SousMenuMusic()
    {
        animator.SetBool("MusicSelected", true);
    }

    public void SousMenuCredits()
    {
        animator.SetBool("CreditsSelected", true);

    }

    public void ResetAll()
    {
        animator.SetBool("SoundSelected", false);
        animator.SetBool("MusicSelected", false);
        animator.SetBool("CreditsSelected", false);
    }
    /*
    if (Sound)
        {
           /* if (animator.GetBool("SoundSelected") == false)
            {
                animator.SetBool("SoundSelected", true);
            //}

            if (animator.GetBool("SoundSelected") == true)
            {
                animator.SetBool("SoundSelected", false);
            }
        }

        if(Music)
        {
            if (animator.GetBool("MusicSelected") == false)
            {
                animator.SetBool("MusicSelected", true);
            }

            if (animator.GetBool("MusicSelected") == true)
            {
                animator.SetBool("MusicSelected", false);
            }
        }

        if(Credits)
        {
            if (animator.GetBool("CreditsSelected") == false)
            {
                animator.SetBool("CreditsSelected", true);
            }

            if (animator.GetBool("CreditsSelected") == true)
            {
                animator.SetBool("CreditsSelected", false);
            }
        }*/
}
