using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harcelement_Manager : MonoBehaviour
{
    public List<Harcelements> harceleurs;



    public static Harcelement_Manager Instance;




    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Lance les voix
    /// </summary>
    public void AmongThem()
    {
        for (int i = 0; i < harceleurs.Count; i++)
        {
            harceleurs[i].gameObject.SetActive(true);
            harceleurs[i].ActivateClientVoice();
        }   
    }

    /// <summary>
    /// Diminue toutes les voix pendant un temps
    /// </summary>
    public void Deaf()
    {
        for (int i = 0; i < harceleurs.Count; i++)
        {
            harceleurs[i].Silence();
        }
    }

    /// <summary>
    /// Arrête les voix
    /// </summary>
    public void FarFromThem()
    {
        for (int i = 0; i < harceleurs.Count; i++)
        {
            harceleurs[i].DeactivateClientVoice();
        }

        StartCoroutine(DelayBeforeDeactivating());
    }



    IEnumerator DelayBeforeDeactivating()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < harceleurs.Count; i++)
        {
            harceleurs[i].gameObject.SetActive(false);
        }
    }
}
