using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(DialogueSystem))]
public class DialogueSetUp : Editor
{
    public GameObject dialogueBoxPrefabReference;
    public GameObject actorIconsPrefabReference;



    public override void OnInspectorGUI()
    {
        DialogueSystem dialogueSystem = (DialogueSystem)target;

        if (GUILayout.Button("Update Dialogue Parameters"))
        {
            dialogueSystem.dialogueBoxPrefab = dialogueBoxPrefabReference;
            dialogueSystem.dialogueGo = dialogueSystem.gameObject.GetComponent<RectTransform>();

            dialogueSystem.actorsIconHierarchyReference = GameObject.Find(actorIconsPrefabReference.name);

            dialogueSystem.SetUp();
        }

        base.OnInspectorGUI();
    }
}
