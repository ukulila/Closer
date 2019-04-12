using UnityEditor;
using UnityEngine;

public class DialogueTool : EditorWindow
{
    public Object yourTextFile;
    public Object[] textFiles = AssetDatabase.LoadAllAssetsAtPath("Assets / Textes");
    //int currentPickerWindow;

    [MenuItem("Gameplay Tools/Create a Dialogue")]
    public static void ShowWindow()
    {
        GetWindow<DialogueTool>("Create a dialogue");
    }

    private void OnGUI()
    {
        GUILayout.Label("Dialogue Text", EditorStyles.boldLabel);
        yourTextFile = EditorGUILayout.ObjectField(yourTextFile, typeof(TextAsset), false);
        //EditorGUIUtility.ShowObjectPicker<TextAsset>(textFiles, false, "ee",currentPickerWindow);


    }
}
