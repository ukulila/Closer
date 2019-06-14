using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRandomMenu : MonoBehaviour
{

    public List<GameObject> particules;
    public List<AudioSource> ambiance;
    public int randomInt;

    public void OnEnable()
    {
        SelectRandomAmbiance();
    }

    public void SelectRandomAmbiance()
    {
        randomInt = Random.Range(0, particules.Count);
        particules[randomInt].SetActive(true);
        ambiance[randomInt].Play();

        for (int i = 0; i < particules.Count; i++)
        {
            if (i != randomInt)
            {
                particules[i].SetActive(false);
                ambiance[i].Stop();
            }
        }
    }
}
