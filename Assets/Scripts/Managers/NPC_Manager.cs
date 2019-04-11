﻿using UnityEngine;

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
        currentNPC.StartDialogueAbout();
    }
}