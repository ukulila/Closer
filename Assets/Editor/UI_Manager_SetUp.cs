using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UI_Manager))]
public class UI_Manager_SetUp : Editor
{
    [Header("Asset Name References")]
    public GameObject dialogueCharacterBox_Ref;
    public GameObject dialogueUImask_Ref;
    [Space]
    public GameObject ui_Contextuelle_Ref;
    public GameObject outOfContext_Ref;
    [Space]
    public GameObject slots_Ref;
    public GameObject outOfUI_Ref;
    public GameObject selectedImage_Ref;
    public GameObject objectif_Ref;
    public GameObject progression_Ref;
    public GameObject inventoryButtons_Ref;
    [Space]
    public GameObject uI_Background_Ref;
    [Space]
    public GameObject Winning_Ref;

    [Header("Current for each scene")]
    public GameObject current_DialogueCharacterBox;
    public GameObject current_DialogueUImask;
    [Space]
    public GameObject current_ui_Contextuelle;
    public GameObject current_outOfContext;
    [Space]
    public GameObject current_slots;
    public GameObject current_outOfUI;
    public GameObject current_selectedImage;
    public GameObject current_objectif;
    public GameObject current_progression;
    public GameObject current_inventoryButtons;
    [Space]
    public GameObject current_UI_Background;
    [Space]
    public GameObject current_Winning;



    public override void OnInspectorGUI()
    {
        UI_Manager ui_Manager = (UI_Manager)target;


        
        if (GUILayout.Button("Set References"))
        {
            ui_Manager.dialogueGO.Clear();
            ui_Manager.contextuelleGO.Clear();
            ui_Manager.outOfContextGO.Clear();
            ui_Manager.inventoryGO.Clear();
            ui_Manager.inventoryButtonsGO.Clear();
            ui_Manager.backgroundGO.Clear();
            ui_Manager.WinningGO.Clear();


            if (GameObject.Find(dialogueCharacterBox_Ref.name))
            {
                current_DialogueCharacterBox = GameObject.Find(dialogueCharacterBox_Ref.name);
            }

            if (GameObject.Find(dialogueCharacterBox_Ref.name + "(Clone)"))
            {
                current_DialogueCharacterBox = GameObject.Find(dialogueCharacterBox_Ref.name + "(Clone)");
            }

            if (GameObject.Find(dialogueUImask_Ref.name))
            {
                current_DialogueUImask = GameObject.Find(dialogueUImask_Ref.name);
            }

            if (GameObject.Find(dialogueUImask_Ref.name + "(Clone)"))
            {
                current_DialogueUImask = GameObject.Find(dialogueUImask_Ref.name + "(Clone)");
            }



            if (GameObject.Find(ui_Contextuelle_Ref.name))
            {
                current_ui_Contextuelle = GameObject.Find(ui_Contextuelle_Ref.name);
            }

            if (GameObject.Find(ui_Contextuelle_Ref.name + "(Clone)"))
            {
                current_ui_Contextuelle = GameObject.Find(ui_Contextuelle_Ref.name + "(Clone)");
            }

            if (GameObject.Find(outOfContext_Ref.name))
            {
                current_outOfContext = GameObject.Find(outOfContext_Ref.name);
            }



            if (GameObject.Find(slots_Ref.name))
            {
                current_slots = GameObject.Find(slots_Ref.name);
            }

            if (GameObject.Find(selectedImage_Ref.name))
            {
                current_selectedImage = GameObject.Find(selectedImage_Ref.name);
            }

            if (GameObject.Find(outOfUI_Ref.name))
            {
                current_outOfUI = GameObject.Find(outOfUI_Ref.name);
            }

            //if (GameObject.Find(objectif_Ref.name))
            //{
            //    current_objectif = GameObject.Find(objectif_Ref.name);
            //}

            //if (GameObject.Find(progression_Ref.name))
            //{
            //    current_progression = GameObject.Find(progression_Ref.name);
            //}

            if (GameObject.Find(inventoryButtons_Ref.name))
            {
                current_inventoryButtons = GameObject.Find(inventoryButtons_Ref.name);
            }



            if (GameObject.Find(uI_Background_Ref.name))
            {
                current_UI_Background = GameObject.Find(uI_Background_Ref.name);
            }



            if (GameObject.Find(Winning_Ref.name))
            {
                current_Winning = GameObject.Find(Winning_Ref.name);
            }





            if (current_DialogueCharacterBox != null)
            {
                ui_Manager.dialogueGO.Add(current_DialogueCharacterBox);
            }

            if (current_DialogueUImask != null)
            {
                ui_Manager.dialogueGO.Add(current_DialogueUImask);
            }



            if (current_ui_Contextuelle != null)
            {
                ui_Manager.contextuelleGO.Add(current_ui_Contextuelle);
            }

            if (current_outOfContext != null)
            {
                ui_Manager.outOfContextGO.Add(current_outOfContext);
            }



            if (current_slots != null)
            {
                ui_Manager.inventoryGO.Add(current_slots);
            }

            if (current_selectedImage != null)
            {
                ui_Manager.inventoryGO.Add(current_selectedImage);
            }

            if (current_outOfUI != null)
            {
                ui_Manager.inventoryGO.Add(current_outOfUI);
            }

            //if (current_objectif != null)
            //{
            //    ui_Manager.inventoryGO.Add(current_objectif);
            //}

            //if (current_progression != null)
            //{
            //    ui_Manager.inventoryGO.Add(current_progression);
            //}

            if (current_inventoryButtons != null)
            {
                ui_Manager.inventoryButtonsGO.Add(current_inventoryButtons);
            }



            if (current_UI_Background != null)
            {
                ui_Manager.backgroundGO.Add(current_UI_Background);
            }



            if (current_Winning != null)
            {
                ui_Manager.WinningGO.Add(current_Winning);
            }


            if (GUI.changed)
                EditorUtility.SetDirty(ui_Manager);
        }


        base.OnInspectorGUI();

    }
}
