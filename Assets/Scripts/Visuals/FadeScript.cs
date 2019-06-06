using System.Collections;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    public Animator fadeAnim;

    public static FadeScript Instance;



    private void Awake()
    {
        Instance = this;

        if (fadeAnim == null)
            fadeAnim = GetComponent<Animator>();
    }

    public void FadeINandOUT()
    {
        //StartCoroutine(DeactivateGameObject());

        fadeAnim.SetTrigger("FadeIn");

        fadeAnim.SetTrigger("FadeOut");
    }
}
