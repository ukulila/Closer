using System.Collections;
using UnityEngine;

public class NPC_Manager : MonoBehaviour
{
    public NPCInteractions currentNPC;


    public static NPC_Manager Instance;



    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Lance la fonction Dialogue du current NPC
    /// </summary>
    public void TalkToCurrentNPC()
    {
        StartCoroutine(StartDialogueAnimIn(1.5f));
        GameManager.Instance.SwitchModeTo(GameManager.GameMode.InteractingMode);
    }

    IEnumerator StartDialogueAnimIn(float time)
    {
        yield return new WaitForSeconds(time);

        currentNPC.StartDialogueAbout();
    }
}
