using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(RoomInteraction))]
public class RoomInteractionSetUp : Editor
{
    [Header("Asset Name References")]
    public GameObject UI;
    public GameObject talk;
    public GameObject interact;
    public GameObject floor;
    public GameObject title;
    public GameObject description;
    public GameObject nothing;
    public GameObject background;

    [Header("Gameobject in Scene")]
    public GameObject currentUI;
    public GameObject currentCanvas;

    public override void OnInspectorGUI()
    {
        RoomInteraction roomInteraction = (RoomInteraction)target;


        if (GUILayout.Button("Get References"))
        {
            if (FindObjectOfType<Canvas>() != null)
            {
                currentCanvas = FindObjectOfType<Canvas>().gameObject;
            }
            else
            {
                GameObject go = new GameObject("Canvas", typeof(Canvas));
                go.AddComponent<GraphicRaycaster>();
                currentCanvas = go;
            }

            if ((GameObject.Find(UI.name + "(Clone)") || GameObject.Find(UI.name)))
            {
                currentUI = FindObjectOfType<Canvas>().gameObject;
            }
            else
            {
                Instantiate(UI, currentCanvas.transform);
            }

            if (roomInteraction.roomName == "")
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.roomName = "Enter a name";
                //EditorUtility.SetDirty(this);
            }


            if (roomInteraction.roomDescription == "")
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.roomDescription = "Enter a description";
                //EditorUtility.SetDirty(this);
            }


            if (roomInteraction.talkTo == null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.talkTo = GameObject.Find(talk.name).GetComponent<Button>();
                //EditorUtility.SetDirty(this);
            }

            if (roomInteraction.interactWith == null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.interactWith = GameObject.Find(interact.name).GetComponent<Button>();
                //EditorUtility.SetDirty(this);
            }

            if (roomInteraction.changeFloor == null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.changeFloor = GameObject.Find(floor.name).GetComponent<Button>();
                //EditorUtility.SetDirty(this);
            }

            if (roomInteraction.nameText == null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.nameText = GameObject.Find(title.name).GetComponent<TextMeshProUGUI>();
                //EditorUtility.SetDirty(this);
            }

            if (roomInteraction.descritpionText == null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.descritpionText = GameObject.Find(description.name).GetComponent<TextMeshProUGUI>();
                //EditorUtility.SetDirty(this);
            }

            if (roomInteraction.nothingText == null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.nothingText = GameObject.Find(nothing.name).GetComponent<TextMeshProUGUI>();
                //EditorUtility.SetDirty(this);
            }

            if (roomInteraction.backgroundSprite == null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.backgroundSprite = GameObject.Find(background.name).GetComponent<SpriteRenderer>();
                //EditorUtility.SetDirty(this);
            }

            roomInteraction.uiAnimators.Clear();

            if (roomInteraction.talkTo != null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.uiAnimators.Add(roomInteraction.talkTo.gameObject.GetComponent<Animator>());
                //EditorUtility.SetDirty(this);
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.interactWith != null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.uiAnimators.Add(roomInteraction.interactWith.gameObject.GetComponent<Animator>());
                //EditorUtility.SetDirty(this);
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.changeFloor != null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.uiAnimators.Add(roomInteraction.changeFloor.gameObject.GetComponent<Animator>());
                //EditorUtility.SetDirty(this);
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.nameText != null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.uiAnimators.Add(roomInteraction.nameText.gameObject.GetComponent<Animator>());
                //EditorUtility.SetDirty(this);
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.descritpionText != null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.uiAnimators.Add(roomInteraction.descritpionText.gameObject.GetComponent<Animator>());
                //EditorUtility.SetDirty(this);
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.nothingText != null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.uiAnimators.Add(roomInteraction.nothingText.gameObject.GetComponent<Animator>());
                //EditorUtility.SetDirty(this);
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.backgroundSprite != null)
            {
                //Undo.RecordObject(this, "Bla bla");
                roomInteraction.uiAnimators.Add(roomInteraction.backgroundSprite.gameObject.GetComponent<Animator>());
                //EditorUtility.SetDirty(this);
            }
            else
                Debug.LogWarning("This Button is not assigned");
        }

        base.OnInspectorGUI();
    }

}
