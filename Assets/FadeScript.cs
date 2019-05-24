using System.Collections;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    public Animator fadeAnim;


    private void Awake()
    {
        if (fadeAnim == null)
            fadeAnim = GetComponent<Animator>();
    }

    public void FadeINandOUT()
    {
        //StartCoroutine(DeactivateGameObject());

        fadeAnim.SetTrigger("FadeIn");

        fadeAnim.SetTrigger("FadeOut");
    }

    //IEnumerator DeactivateGameObject()
    //{
    //    fadeAnim.SetTrigger("FadeIn");

    //    yield return new WaitForSeconds(1.5f);

    //    fadeAnim.SetTrigger("FadeOut");
    //}
}
