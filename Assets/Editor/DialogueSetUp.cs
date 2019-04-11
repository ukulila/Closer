using UnityEngine;
using UnityEditor;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(DialogueSystem))]
public class DialogueSetUp : Editor
{
    public GameObject dialogueBoxPrefabReference;

    public override void OnInspectorGUI()
    {
        DialogueSystem dialogueSystem = (DialogueSystem)target;


        if (GUILayout.Button("Get Prefabs"))
        {
            dialogueSystem.dialogueBoxPrefab = dialogueBoxPrefabReference;
            dialogueSystem.dialogueGo = dialogueSystem.gameObject.GetComponent<RectTransform>();
        }


        if (GUILayout.Button("Update Dialogue Parameters"))
        {
            dialogueSystem.CleanDialogueSetUp();
            dialogueSystem.SetUpTextFile();
            dialogueSystem.SetUpDialogueLines();
            dialogueSystem.SetUpDialogueBox();
            dialogueSystem.SetActorsParameters();
        }

        base.OnInspectorGUI();
    }
}
