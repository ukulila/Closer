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

    [Header("Gameobject in Scene")]
    public GameObject currentUI;
    public GameObject currentCanvas;

    public override void OnInspectorGUI()
    {
        RoomInteraction roomInteraction = (RoomInteraction)target;


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






        if (GUILayout.Button("Get References"))
        {
            if(roomInteraction.talkTo == null)
            {
                roomInteraction.talkTo = GameObject.Find(talk.name).GetComponent<Button>(); ;
            }

            if (roomInteraction.interactWith == null)
            {
                roomInteraction.interactWith = GameObject.Find(interact.name).GetComponent<Button>(); ;
            }

            if (roomInteraction.changeFloor == null)
            {
                roomInteraction.changeFloor = GameObject.Find(floor.name).GetComponent<Button>(); ;
            }

            if (roomInteraction.nameText == null)
            {
                roomInteraction.nameText = GameObject.Find(title.name).GetComponent<TextMeshProUGUI>(); ;
            }

            if (roomInteraction.descritpionText == null)
            {
                roomInteraction.descritpionText = GameObject.Find(description.name).GetComponent<TextMeshProUGUI>(); ;
            }


            roomInteraction.SetAnimators();
        }

        base.OnInspectorGUI();
    }
}
