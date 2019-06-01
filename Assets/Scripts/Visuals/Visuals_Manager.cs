using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visuals_Manager : MonoBehaviour
{
    public List<GameObject> StandardObjects;
    public List<GameObject> GhostObjects;
    

    void Update()
    {
        if(GameManager.Instance.currentGameMode == GameManager.GameMode.InvestigationMode || GameManager.Instance.currentGameMode == GameManager.GameMode.Dialogue)
        {
            for (int i = 0; i < StandardObjects.Count; i++)
            {
                StandardObjects[i].SetActive(false);
                GhostObjects[i].SetActive(true);
            }
        }
        else
        {
            if (StandardObjects[StandardObjects.Count - 1].activeInHierarchy == false)
            {
                for (int i = 0; i < StandardObjects.Count; i++)
                {
                    StandardObjects[i].SetActive(true);
                    GhostObjects[i].SetActive(false);
                }
            }

        }
    }
}
