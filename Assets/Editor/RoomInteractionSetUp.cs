using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[CanEditMultipleObjects]
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
                roomInteraction.roomName = "Enter a name";
            }


            if (roomInteraction.roomDescription == "")
            {
                roomInteraction.roomDescription = "Enter a description";
            }


            if (roomInteraction.talkTo == null)
            {
                roomInteraction.talkTo = GameObject.Find(talk.name).GetComponent<Button>();
            }

            if (roomInteraction.interactWith == null)
            {
                roomInteraction.interactWith = GameObject.Find(interact.name).GetComponent<Button>();
            }

            if (roomInteraction.changeFloor == null)
            {
                roomInteraction.changeFloor = GameObject.Find(floor.name).GetComponent<Button>();
            }

            if (roomInteraction.nameText == null)
            {
                roomInteraction.nameText = GameObject.Find(title.name).GetComponent<TextMeshProUGUI>();
            }

            if (roomInteraction.descritpionText == null)
            {
                roomInteraction.descritpionText = GameObject.Find(description.name).GetComponent<TextMeshProUGUI>();
            }

            if (roomInteraction.nothingText == null)
            {
                roomInteraction.nothingText = GameObject.Find(nothing.name).GetComponent<TextMeshProUGUI>();
            }

            if (roomInteraction.backgroundSprite == null)
            {
                roomInteraction.backgroundSprite = GameObject.Find(background.name).GetComponent<SpriteRenderer>();
            }

            roomInteraction.uiAnimators.Clear();

            if (roomInteraction.talkTo != null)
            {
                roomInteraction.uiAnimators.Add(roomInteraction.talkTo.gameObject.GetComponent<Animator>());
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.interactWith != null)
            {
                roomInteraction.uiAnimators.Add(roomInteraction.interactWith.gameObject.GetComponent<Animator>());
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.changeFloor != null)
            {
                roomInteraction.uiAnimators.Add(roomInteraction.changeFloor.gameObject.GetComponent<Animator>());
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.nameText != null)
            {
                roomInteraction.uiAnimators.Add(roomInteraction.nameText.gameObject.GetComponent<Animator>());
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.descritpionText != null)
            {
                roomInteraction.uiAnimators.Add(roomInteraction.descritpionText.gameObject.GetComponent<Animator>());
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.nothingText != null)
            {
                roomInteraction.uiAnimators.Add(roomInteraction.nothingText.gameObject.GetComponent<Animator>());
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (roomInteraction.backgroundSprite != null)
            {
                roomInteraction.uiAnimators.Add(roomInteraction.backgroundSprite.gameObject.GetComponent<Animator>());
            }
            else
                Debug.LogWarning("This Button is not assigned");

            if (GUI.changed)
                EditorUtility.SetDirty(roomInteraction);
        }

        base.OnInspectorGUI();
    }

}
