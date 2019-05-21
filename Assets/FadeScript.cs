using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(DeactivateGameObject());
    }

    IEnumerator DeactivateGameObject()
    {
        fadeAnim.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1.5f);

        fadeAnim.SetTrigger("FadeOut");
    }
}
