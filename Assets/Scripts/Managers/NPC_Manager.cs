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
        //UI_Manager.Instance.ActivateListOfUI(UI_Manager.Instance.dialogueGO);

        currentNPC.StartDialogueAbout();
        GameManager.Instance.currentGameMode = GameManager.GameMode.InteractingMode;
    }
}
