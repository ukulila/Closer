using UnityEngine;
using UnityEditor;
using System.Collections.Generic;




// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(DialogueSystem))]
public class DialogueSetUp : Editor
{
    public GameObject dialogueBoxPrefabReference;
    public GameObject actorIconsPrefabReference;

    public Color speakingColor_Reference = new Color32(255, 255, 255, 255);
    public Color listeningColor_Reference = new Color32(140, 140, 140, 255);




    public override void OnInspectorGUI()
    {
        DialogueSystem dialogueSystem = (DialogueSystem)target;

        if (GUILayout.Button("Update Dialogue Parameters"))
        {
            dialogueSystem.dialogueBoxPrefab = dialogueBoxPrefabReference;
            dialogueSystem.dialogueGo = dialogueSystem.gameObject.GetComponent<RectTransform>();



            if (GameObject.Find(actorIconsPrefabReference.name))
                dialogueSystem.actorsIconHierarchyReference = GameObject.Find(actorIconsPrefabReference.name);

            if (GameObject.Find(actorIconsPrefabReference.name + "(Clone)"))
                dialogueSystem.actorsIconHierarchyReference = GameObject.Find(actorIconsPrefabReference.name + "(Clone)");

            dialogueSystem.speakingColor = speakingColor_Reference;
            dialogueSystem.listeningColor = listeningColor_Reference;

            dialogueSystem.CleanDialogueSetUp();
            dialogueSystem.SetUp();
        }

        base.OnInspectorGUI();
    }
}
