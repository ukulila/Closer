using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectif_Scr : MonoBehaviour
{
    public string ObjectifText;
    public TMP_Text myTextVar;
    public Animator myAnimator;
    public bool test;
    public bool test0;
    public bool test2;
    public bool test3;

    public void RenameObjectif()
    {
        // = ObjectifText;
        myTextVar.text = ObjectifText;
    }

    public void RePlayAnim()
    {

        myAnimator.SetBool("RePlay", true);
        return;

    }

    public void Update()
    {

        if (test)
        {
            RenameObjectif();
            RePlayAnim();

        }
        if(test0)
        {
            ResetAnimPosition();
        }


        if(test2)
        {

            Disappearance();
        }
        if(test3)
        {
            Appearance();
        }

    }

    public void Disappearance()
    {
        myAnimator.SetBool("RePlay", false);
        myAnimator.SetBool("FadeOut", true);
        return;

    }
    public void Appearance()
    {

        myAnimator.SetBool("FadeOut", false);
        return;

    }

    public void ResetAnimPosition()
    {
        myAnimator.SetBool("RePlay", false);
        return;

    }

}
