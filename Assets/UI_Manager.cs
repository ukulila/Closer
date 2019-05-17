using System.Collections.Generic;
using System.Collections;
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
    public List<GameObject> outOfContextGO;

    [Header("UI_Background")]
    public List<GameObject> backgroundGO;

    [Header("UI_Winning")]
    public List<GameObject> WinningGO;



    public static UI_Manager Instance;



    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Désactivation de GameObject
    /// </summary>
    /// <param name="goToDeactivate">List de GameObject à désactiver</param>
    public void DeactivateListOfUI(List<GameObject> goToDeactivate)
    {
        for (int i = 0; i < goToDeactivate.Count; i++)
        {
            goToDeactivate[i].SetActive(false);
        }
    }

    /// <summary>
    /// Activation de GameObject
    /// </summary>
    /// <param name="goToActivate">List de GameObject à activer</param>
    public void ActivateListOfUI(List<GameObject> goToActivate)
    {
        for (int i = 0; i < goToActivate.Count; i++)
        {
            goToActivate[i].SetActive(true);
        }
    }

    /// <summary>
    /// Désactive les GO après un delay
    /// </summary>
    /// <param name="delay">Delay avant désactivation</param>
    /// <param name="goToDeactivate">List de GameObject à désactiver</param>
    /// <returns></returns>
    public IEnumerator DelayBeforeDeactivation(float delay, List<GameObject> goToDeactivate)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < goToDeactivate.Count; i++)
        {
            goToDeactivate[i].SetActive(false);
        }
    }
}
