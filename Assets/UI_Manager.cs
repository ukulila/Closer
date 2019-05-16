using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [Header("UI_Dialogue")]
    public List<GameObject> dialogueGO;

    [Header("UI_Inventory")]
    public List<GameObject> inventoryGO;
    public List<GameObject> inventoryButtonsGO;

    [Header("UI_Contextuelle")]
    public List<GameObject> contextuelleGO;

    [Header("UI_Background")]
    public List<GameObject> backgroundGO;

    [Header("UI_Winning")]
    public List<GameObject> WinningGO;



    public static UI_Manager Instance;



    private void Awake()
    {
        Instance = this;
    }


    public void DeactivateListOfUI(List<GameObject> goToDeactivate)
    {
        for (int i = 0; i < goToDeactivate.Count; i++)
        {
            goToDeactivate[i].SetActive(false);
        }
    }

    public void ActivateListOfUI(List<GameObject> goToActivate)
    {
        for (int i = 0; i < goToActivate.Count; i++)
        {
            goToActivate[i].SetActive(true);
        }
    }
}
