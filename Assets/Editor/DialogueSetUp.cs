using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(DialogueSystem))]
public class DialogueSetUp : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueSystem dialogueSystem = (DialogueSystem)target;

        if(GUILayout.Button("Generate Dialogue"))
        {
            
        }
    }
}
