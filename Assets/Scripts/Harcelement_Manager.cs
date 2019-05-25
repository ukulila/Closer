using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harcelement_Manager : MonoBehaviour
{
    [Header("Harcelement Management")]
    public List<Harcelements> harceleurs;
    public bool isHarcelementOn;


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
        if (!isHarcelementOn)
        {
            for (int i = 0; i < harceleurs.Count; i++)
            {
                harceleurs[i].gameObject.SetActive(true);
                harceleurs[i].ActivateClientVoice();
            }

            isHarcelementOn = true;
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
        if (isHarcelementOn)
        {
            for (int i = 0; i < harceleurs.Count; i++)
            {
                harceleurs[i].DeactivateClientVoice();
            }

            isHarcelementOn = false;

            StartCoroutine(DelayBeforeDeactivating());
        }
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
