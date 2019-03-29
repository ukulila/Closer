using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(DialogueSystem))]
public class DialogueSetUp : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueSystem dialogueSystem = (DialogueSystem)target;

        if (GUILayout.Button("Update Dialogue Parameters"))
        {
            dialogueSystem.CleanDialogueSetUp();
            dialogueSystem.SetUpTextFile();
            dialogueSystem.SetUpDialogueLines();
            dialogueSystem.SetUpDialogueBox();
        }

        base.OnInspectorGUI();
    }
}
