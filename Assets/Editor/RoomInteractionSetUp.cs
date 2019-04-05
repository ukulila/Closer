using UnityEngine;
using UnityEditor;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(RoomInteraction))]
public class RoomInteractionSetUp : Editor
{

    public override void OnInspectorGUI()
    {
        RoomInteraction roomInteraction = (RoomInteraction)target;

        if (GUILayout.Button("Update Animators"))
        {
            roomInteraction.SetAnimators();
        }

        base.OnInspectorGUI();
    }
}
