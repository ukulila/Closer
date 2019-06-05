using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CellScript : MonoBehaviour
{
    [Header("   Tweaking Values")]
    [Tooltip("Checker Avec le prog Gameplay pour le tweak de cette value")]
    [Range(0.1f, 10)]
    public float speed = 3;
    [Space(10)]
    public float fin;
    [Space(10)]
    public int timeBetweenTwoTouches = 30;

    [Space(5)]
    [Header("   InGame Values [Don't touch it]")]
    public bool isInRotation;
    public int timer;
    private bool first;
    //private int nbrTouch;
    private bool set;
    private Quaternion myRot;
    private int timeRot = 0;

    [Space(10)]
    [Header("   ***** Values : Need Assignment [See Tooltip] *****")]
    [Space(5)]
    [Tooltip("Toutes les autres cells (A_..., B_...)")]
    public List<CellMovement> brothers;
    [Space(10)]
    [Tooltip("Le cube en lui même")]
    public CellPlacement cP;
    [Space(10)]
    [Tooltip("La Camera")]
    public CinemachineBrain myBrain;
    [Space(10)]
    [Tooltip("Le player")]
    public PlayerBehaviour player;


    public List<GameObject> coneRed;
    //public List<GameObject> coneGreen;
    public bool freeRoom;
    public List<Material> material;
    private Vector3 rotationVector = new Vector3(0, 90, 0);
    public bool playerMoving;
    public bool isInRotationInverse;

    void Start()
    {
        first = false;
        freeRoom = false;

        
       /* for (int i = 0; i < coneRed.Count; i++)
        {
            material.Add(coneRed[i].GetComponent<Renderer>().material);
            material[i].SetColor("_EmissionColor", Color.red);
        }*/
        
    }

    void Update()
    {
        if (GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode && Camera_Zoom.Instance.onZoom == false)
        {
            if (first)
            {
                timer++;

                if (timer >= timeBetweenTwoTouches)
                {
                    //nbrTouch = 0;
                    timer = 0;
                    first = false;
                }

            }

            if (isInRotation)
            {
                transform.Rotate(new Vector3(0, 1, 0), speed);

                if (cP != null)
                {
                    cP.isInRotation = true;
                }

                timeRot++;

                if (timeRot > fin)
                {
                    timeRot = 0;
                    isInRotation = false;
                    GetComponent<CellMovement>().selected = true;

                    if (cP != null)
                    {
                        cP.isInRotation = false;
                    }

                    if (brothers[0] != null)
                    {
                        for (int r = 0; r < brothers.Count; r++)
                        {
                            brothers[r].hasEnded = true;
                            brothers[r].isOpen = false;
                            player.checkOpenDoor = true;
                        }
                    }
                }
            }

            if (isInRotationInverse)
            {
                transform.Rotate(new Vector3(0, -1, 0), speed);

                if (cP != null)
                {
                    cP.isInRotation = true;
                }

                timeRot++;

                if (timeRot > fin)
                {
                    timeRot = 0;
                    // GetComponent<CellMovement>().OutlineOff = false;

                    GetComponent<CellMovement>().selected = true;

                    isInRotationInverse = false;

                    if (cP != null)
                    {
                        cP.isInRotation = false;
                    }

                    if (brothers[0] != null)
                    {
                        for (int r = 0; r < brothers.Count; r++)
                        {
                            brothers[r].hasEnded = true;
                            brothers[r].isOpen = false;
                            player.checkOpenDoor = true;
                        }
                    }
                }
            }


           /* if (Input.GetMouseButtonDown(0) && !playerMoving)
            {
                RaycastHit hit;
                int LayerMaskCells = LayerMask.GetMask("Cell");

                if (Physics.Raycast(myBrain.OutputCamera.ScreenPointToRay(Input.mousePosition), out hit, 250, LayerMaskCells))
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        first = true;
                        nbrTouch += 1;


                        if (first && nbrTouch > 1)
                        {
                           /* if (brothers[0] != null)
                            {
                                 for (int r = 0; r < brothers.Count; r++)
                                 {
                                     brothers[r].hasEnded = false;
                                 }
                            }
                            
                            set = true;
                            if (set)
                            {
                                myRot = transform.rotation;
                                set = false;
                            }
                            isInRotation = true;
                            nbrTouch = 0;
                        }

                    }

                }

            }*/

        }
    }

    /*public void ConeFunction(int door)
    {
        if (freeRoom)
        {
            material[door].SetColor("_EmissionColor", Color.green);
        }


        if (!freeRoom)
        {
            material[door].SetColor("_EmissionColor", Color.red);
        }
    }*/
    /*
    public void TurnLeft()
    {

        transform.localEulerAngles = new Vector3(transform.rotation.x, Mathf.Lerp(transform.rotation.y, transform.rotation.y + 90, 0.3f), transform.rotation.z);

    }*/
}
