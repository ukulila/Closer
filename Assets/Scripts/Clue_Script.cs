using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Reload()
    {
        StartCoroutine(DelayBeforeReload());
    }


    IEnumerator DelayBeforeClue()
    {
        DataManager.Instance.Save();

        GameManager.Instance.SwitchModeTo(GameManager.GameMode.ClueMode);

        yield return new WaitForSeconds(1.5f);

        clueAnim.SetTrigger("FadeIn");
    }

    IEnumerator DelayBeforeGoingBack()
    {
        FadeScript.Instance.fadeAnim.SetTrigger("FadeIn");

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(0);
    }

    IEnumerator DelayBeforeReload()
    {
        FadeScript.Instance.fadeAnim.SetTrigger("FadeIn");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
