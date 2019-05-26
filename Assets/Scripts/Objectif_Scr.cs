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


    public static Objectif_Scr Instance;



    private void Awake()
    {
        Instance = this;
        myTextVar.text = ObjectifText;
    }


    public void RenameObjectif(string newObjectif)
    {
        myTextVar.text = newObjectif;
    }


    public void Disappearance()
    {
        myAnimator.ResetTrigger("FadeOut");
        myAnimator.ResetTrigger("FadeIn");


        myAnimator.SetTrigger("FadeOut");
        return;
    }


    public void Appearance()
    {
        myAnimator.ResetTrigger("FadeOut");
        myAnimator.ResetTrigger("FadeIn");

        myAnimator.SetTrigger("FadeIn");
        return;
    }


    public void RenameWhileVisible(string newObjectif)
    {
        StartCoroutine(RenameAfterFading(newObjectif));
    }


    IEnumerator RenameAfterFading(string newObjectif)
    {
        myAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        myTextVar.text = newObjectif;

        myAnimator.SetTrigger("FadeIn");
    }
}
