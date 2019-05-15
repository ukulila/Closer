using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum DialogueType { ChéPo, Commun, Quest };

public class DialogueTool : EditorWindow
{
    public Object yourTextFile;
    string dialogueName;

    public Object selectedNPC;

    public GameObject currentCanvas;
    public bool isThereCanvas;

    public GameObject actorIcons_Reference;
    public GameObject currrentActorIconsInHierarchy;
    public bool isThereActorIcons;

    public GameObject NPC_ManagerReference;
    public bool isThereNPC_Manager;

    public GameObject ROOM_ManagerReference;
    public bool isThereROOM_Manager;

    public GameObject dialogueNameBox_Reference;
    public GameObject currentDialogueNameBox;
    public bool isThereDialogueNameBox;

    public GameObject UImask_Reference;
    public GameObject currentUImask;
    public bool isThereUImask;

    public DialogueType type;

    public NPCInteractions npcSelected;

    public GameObject dialogueBoxPrefab_Reference;

    public Color speakingColor_Reference = new Color32(255, 255, 255, 255);
    public Color listeningColor_Reference = new Color32(120, 120, 120, 255);




    [MenuItem("Gameplay Tools/Create a Dialogue")]
    public static void ShowWindow()
    {
        GetWindow<DialogueTool>("Create a dialogue");
    }

    private void OnGUI()
    {
        GUILayout.Label("Dialogue Text", EditorStyles.boldLabel);
        yourTextFile = EditorGUILayout.ObjectField(yourTextFile, typeof(TextAsset), false);

        selectedNPC = EditorGUILayout.ObjectField("NPC déclencheur", selectedNPC, typeof(NPCInteractions), true);

        GUILayout.Label("Sujet de la conversation", EditorStyles.boldLabel);
        dialogueName = EditorGUILayout.TextField(dialogueName);



        type = (DialogueType)EditorGUILayout.EnumPopup(type);




        if (GUILayout.Button("Generate"))
        {

            if (Selection.activeGameObject != null)
            {
                GameObject selectGO = Selection.activeGameObject;

                if (selectGO.GetComponent<NPCInteractions>() != null)
                {
                    npcSelected = selectGO.GetComponent<NPCInteractions>();
                }
            }

            if (FindObjectOfType<Canvas>() != null)
            {
                currentCanvas = FindObjectOfType<Canvas>().gameObject;
                isThereCanvas = true;
            }
            else
            {
                isThereCanvas = false;
            }


            if (GameObject.Find(actorIcons_Reference.name + "(Clone)") || GameObject.Find(actorIcons_Reference.name))
            {
                isThereActorIcons = true;
            }
            else
            {
                isThereActorIcons = false;
            }

            if (GameObject.Find(dialogueNameBox_Reference.name + "(Clone)") || GameObject.Find(dialogueNameBox_Reference.name))
            {
                isThereDialogueNameBox = true;
            }
            else
            {
                isThereDialogueNameBox = false;
            }

            if (GameObject.Find(UImask_Reference.name + "(Clone)") || GameObject.Find(UImask_Reference.name))
            {
                isThereUImask = true;
            }
            else
            {
                isThereUImask = false;
            }

            if (GameObject.Find(NPC_ManagerReference.name + "(Clone)") || GameObject.Find(NPC_ManagerReference.name))
            {
                isThereNPC_Manager = true;
            }
            else
            {
                isThereNPC_Manager = false;
            }


            if (GameObject.Find(ROOM_ManagerReference.name + "(Clone)") || GameObject.Find(ROOM_ManagerReference.name))
            {
                isThereROOM_Manager = true;
            }
            else
            {
                isThereROOM_Manager = false;
            }

            if (selectedNPC != null)
            {
                npcSelected = (NPCInteractions)selectedNPC;
            }


            if (!isThereCanvas)
            {
                GameObject go = new GameObject("Canvas", typeof(Canvas));
                go.AddComponent<GraphicRaycaster>();
                currentCanvas = go;
            }

            if (!isThereActorIcons)
            {
                if (currentCanvas != null)
                    Instantiate(actorIcons_Reference, currentCanvas.transform);

                currrentActorIconsInHierarchy = GameObject.Find(actorIcons_Reference.name + "(Clone)");

                isThereActorIcons = true;
            }
            else
            {
                currrentActorIconsInHierarchy = GameObject.Find(actorIcons_Reference.name);
            }

            if (!isThereDialogueNameBox)
            {
                if (currentCanvas != null)
                    Instantiate(dialogueNameBox_Reference, currentCanvas.transform);

                currentDialogueNameBox = GameObject.Find(dialogueNameBox_Reference.name + "(Clone)");

                isThereDialogueNameBox = true;
            }
            else
            {
                currentDialogueNameBox = GameObject.Find(dialogueNameBox_Reference.name);
            }


            if (!isThereUImask)
            {
                if (currentCanvas != null)
                    Instantiate(UImask_Reference, currentCanvas.transform);

                currentUImask = GameObject.Find(UImask_Reference.name + "(Clone)");

                isThereUImask = true;
            }
            else
            {
                currentUImask = GameObject.Find(UImask_Reference.name);
            }


            if (!isThereNPC_Manager)
            {
                if (currentCanvas != null)
                    Instantiate(NPC_ManagerReference);

                isThereNPC_Manager = true;
            }

            if (!isThereROOM_Manager)
            {
                if (currentCanvas != null)
                    Instantiate(ROOM_ManagerReference);

                isThereROOM_Manager = true;
            }

            if (yourTextFile != null && npcSelected != null && type != DialogueType.ChéPo && currentCanvas != null)
            {
                if (dialogueName == "")
                {
                    dialogueName = "NewDialogue";
                }

                GameObject newDialogue = new GameObject(selectedNPC.name + "_" + type.ToString() + "_" + dialogueName, typeof(RectTransform));
                newDialogue.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

                if (currentUImask != null)
                {
                    newDialogue.transform.parent = currentUImask.transform;
                }
                else
                {
                    newDialogue.transform.parent = currentCanvas.transform;
                }

                newDialogue.GetComponent<RectTransform>().localPosition = new Vector2(335, 72);
                newDialogue.AddComponent<DialogueSystem>();
                newDialogue.GetComponent<DialogueSystem>().asset = (TextAsset)yourTextFile;
                newDialogue.GetComponent<DialogueSystem>().dialogueBoxPrefab = dialogueBoxPrefab_Reference;
                newDialogue.GetComponent<DialogueSystem>().dialogueGo = newDialogue.GetComponent<RectTransform>();

                if (type == DialogueType.Commun)
                    npcSelected.GetComponent<NPCInteractions>().communDialogueSystems.Add(newDialogue.GetComponent<DialogueSystem>());

                if (type == DialogueType.Quest)
                {
                    Undo.RecordObject(this, "Bla bla");
                    npcSelected.GetComponent<NPCInteractions>().questDialogueSystems.Add(newDialogue.GetComponent<DialogueSystem>());
                    npcSelected.GetComponent<NPCInteractions>().questTriggers.Add(new UnityEngine.Events.UnityEvent());
                    EditorUtility.SetDirty(this);
                }



                if (GameObject.Find(actorIcons_Reference.name))
                    newDialogue.GetComponent<DialogueSystem>().actorsIconHierarchyReference = GameObject.Find(actorIcons_Reference.name);

                if (GameObject.Find(actorIcons_Reference.name + "(Clone)"))
                    newDialogue.GetComponent<DialogueSystem>().actorsIconHierarchyReference = GameObject.Find(actorIcons_Reference.name + "(Clone)");


                if (GameObject.Find(dialogueNameBox_Reference.name))
                {
                    newDialogue.GetComponent<DialogueSystem>().nameBox = GameObject.Find(dialogueNameBox_Reference.name).GetComponent<Image>();
                    newDialogue.GetComponent<DialogueSystem>().nameCharacter = GameObject.Find(dialogueNameBox_Reference.name).GetComponentInChildren<TextMeshProUGUI>();
                }

                if (GameObject.Find(dialogueNameBox_Reference.name + "(Clone)"))
                {
                    newDialogue.GetComponent<DialogueSystem>().nameBox = GameObject.Find(dialogueNameBox_Reference.name + "(Clone)").GetComponent<Image>();
                    newDialogue.GetComponent<DialogueSystem>().nameCharacter = GameObject.Find(dialogueNameBox_Reference.name + "(Clone)").GetComponentInChildren<TextMeshProUGUI>();
                }

                newDialogue.GetComponent<DialogueSystem>().speakingColor = speakingColor_Reference;
                newDialogue.GetComponent<DialogueSystem>().listeningColor = listeningColor_Reference;


                yourTextFile = null;
                npcSelected = null;
                dialogueName = null;
                type = DialogueType.ChéPo;

                newDialogue.GetComponent<DialogueSystem>().SetUp();
            }

        }

    }
}
