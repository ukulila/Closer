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


    private void Update()
    {
        if (currentNPC != null)
        {
            if (currentNPC.questDialogueSystems[currentNPC.dialogueIndex].isDialogueFinished == true && currentNPC.questDialogueSystems[currentNPC.dialogueIndex].ending == true
                && currentNPC.questDialogueSystems[currentNPC.dialogueIndex].dialogueBoxReady == true && currentNPC.questDialogueSystems[currentNPC.dialogueIndex].dialogueHasStarted == true)
            {
                if (currentNPC.hasEventBeenInvoked == false)
                {
                    currentNPC.questTriggers[currentNPC.dialogueIndex].Invoke();
                    currentNPC.hasEventBeenInvoked = true;
                }

            }
        }
    }

    /// <summary>
    /// Lance la fonction Dialogue du current NPC
    /// </summary>
    public void TalkToCurrentNPC()
    {
        StartCoroutine(StartDialogueAnimIn(1.5f));
        GameManager.Instance.SwitchModeTo(GameManager.GameMode.Dialogue);
    }

    IEnumerator StartDialogueAnimIn(float time)
    {
        yield return new WaitForSeconds(time);

        currentNPC.StartDialogueAbout();
    }
}
