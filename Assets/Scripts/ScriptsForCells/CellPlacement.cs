using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CellPlacement : MonoBehaviour
{
    [Header("   ***** Values : Need Assignment *****")]
    public List<CellMovement> cM;
    public CinemachineBrain myBrain;
    public CameraBehaviour cB;
    public Camera_UI Cui;
    public Camera_Rotation Cr;

    [Header("   Bool Manager")]
    public bool once;
    public bool doOnce;
    public bool okToSetup;
    public bool isInRotation;

    public GameObject facingPlane;
    public bool FingerOnScreen;

    public int timerObjectif;
    public Objectif_Scr objectif;

    public bool DesactCells;
    public static CellPlacement Instance;
    public int speed;


    /*
    public bool playHasEndedSound;
    public bool playSelectionSound;
    public bool playIsOpenSound;
    public bool playMovingRooms;
    public bool playUnsSlectionSound;
    */
    [Header("   Sounds")]

    public AudioSource LeMouvementSeTermine;
    public AudioSource SelectionDUnePièce;
    public AudioSource UnePièceEstOpen;
    public List<AudioSource> DéplacementDesPièces;
    public AudioSource DésélectionDUnePièce;
    public AudioSource ClicQuandDeuxPortesSeFontFace;
    public List<AudioSource> RotationDesPièces;

    public void Awake()
    {
        doOnce = true;

        Instance = this;
        Application.targetFrameRate = 400;

    }

    void Update()
    {

        /*   if (playHasEndedSound)
           {
               PlayHasEndedSound();
               playHasEndedSound = false;
           }

           if (playSelectionSound)
           {
               PlaySelectionSound();
               playSelectionSound = false;
           }*/

        if (GameManager.Instance.currentGameMode == GameManager.GameMode.Dialogue || GameManager.Instance.currentGameMode == GameManager.GameMode.CinematicMode)
        {
            if (doOnce)
            {
                DesactCells = true;
                doOnce = false;
            }

            if (DesactCells == true)
            {

                DeActivateCells();

            }
        }
        else
        {
            ReactivateCells();
        }




        if (GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
        {
            TimerObjectif();
        }
        else
        {
            timerObjectif = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {

            /*  RaycastHit hitAnywhere;

              if (Physics.Raycast(myBrain.OutputCamera.ScreenPointToRay(Input.mousePosition), out hitAnywhere, 250))
              {
                  Debug.Log(hitAnywhere.transform.name);
              }
              */
            if (GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
            {
                FingerOnScreen = true;
            }

            once = true;

            RaycastHit hit;
            int LayerMaskCells = LayerMask.GetMask("Cell");

            if (Physics.Raycast(myBrain.OutputCamera.ScreenPointToRay(Input.mousePosition), out hit, 250, LayerMaskCells))
            {

                if (hit.collider.gameObject.GetComponent<CellMovement>() != null)
                {
                    CellMovement cellmove = hit.collider.gameObject.GetComponent<CellMovement>();

                    cellmove.click = true;
                    cellmove.over = true;

                    for (int i = 0; i < cM.Count; i++)
                    {
                        if (cM[i] != cellmove && !cM[i].isSpawn)
                        {
                            cM[i].unSelection = true;
                        }
                    }

                    if (Cui.switchToUI == true)
                    {
                        //cellmove.hasEnded = false;
                    }

                    if (GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
                    {
                        if (cellmove.isSpawn == true)
                        {
                            /*for (int i = 0; i < cM.Count; i++)
                            {
                                cM[i].isOpen = false;
                            }*/
                            cellmove.raycastAutor = true;
                        }




                        if (cellmove.isOpen)
                        {
                            cellmove.clickDirection = true;
                        }

                        cellmove.originPos = Input.mousePosition;


                    }

                    if (cB != null)
                        cB.aboutCamera = false;

                    if (Cr != null)
                        Cr.aboutCamera = false;

                    //   trail_Behaviour.Instance.DeactivateTrail();
                }




                if (hit.collider.gameObject.GetComponent<CellScript>() != null)
                {
                    CellScript cellscr = hit.collider.gameObject.GetComponent<CellScript>();

                    if (Cui.switchToUI == true)
                    {
                        cellscr.isInRotation = false;
                    }
                }
            }
            else
            {
                // camM.aboutCamera = true;

                if (Cui.switchToUI == false && cB != null)
                    cB.aboutCamera = true;

                if (Cui.switchToUI == false && Cr != null)
                    Cr.aboutCamera = true;

                for (int i = 0; i < cM.Count; i++)
                {

                    cM[i].over = false;

                    if (cM[i].selected)
                    {
                        cM[i].unSelection = true;
                        cM[i].isOpen = false;
                    }
                }


            }
        }


        if (Input.GetMouseButtonUp(0) && GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
        {

            FingerOnScreen = false;

        }



        if (Input.GetMouseButton(0) && once)
        {
            RaycastHit hit;

            int layerMaskPlanes = LayerMask.GetMask("Planes");

            if (Physics.Raycast(myBrain.OutputCamera.ScreenPointToRay(Input.mousePosition), out hit, 250, layerMaskPlanes))
            {
                facingPlane = hit.transform.gameObject;
                once = false;
            }

        }

    }


    public void CellsDeSelection()
    {
        for (int i = 0; i < cM.Count; i++)
        {
            if (!cM[i].isSpawn)
            {
                cM[i].selected = false;

            }

        }
    }

    private void DeActivateCells()
    {

        for (int i = 0; i < cM.Count; i++)
        {
            if (!cM[i].isSpawn)
            {
                cM[i].gameObject.SetActive(false);

            }
        }
    }

    public void ReactivateCells()
    {
        DesactCells = false;

        for (int i = 0; i < cM.Count; i++)
        {
            if (cM[i].gameObject.activeInHierarchy == false)
            {

                cM[i].gameObject.SetActive(true);

            }
        }

    }


    public void TimerObjectif()
    {

        if (!FingerOnScreen)
        {
            timerObjectif++;
        }
        else
        {
            objectif.Disappearance();
            timerObjectif = 0;
        }


        if (timerObjectif > 200)
        {
            objectif.Appearance();
        }

    }


    public void turnLeft()
    {

        for (int i = 0; i < cM.Count; i++)
        {
            if (cM[i].selected && cM[i].isSpawn == false && cM[i].GetComponent<CellScript>().isInRotationInverse == false)
            {/*
                cM[i].OutlineOff = false;

                cM[i].selected = false;
                cM[i].Outline.gameObject.SetActive(true);
                cM[i].CanvasRotation.gameObject.SetActive(true);

                cM[i].unSelection = false;
                cM[i].timerUnSelection = 0;

                //Debug.Log(cM[i].gameObject.name);
                cM[i].GetComponent<CellScript>().isInRotation = true;
                */
                cM[i].RotationButtonLeft();

            }
        }

    }

    public void turnRight()
    {

        for (int i = 0; i < cM.Count; i++)
        {
            if (cM[i].selected && cM[i].isSpawn == false && cM[i].GetComponent<CellScript>().isInRotation == false)
            {/*
                cM[i].OutlineOff = false;

                cM[i].selected = false;
                cM[i].Outline.gameObject.SetActive(true);
                cM[i].CanvasRotation.gameObject.SetActive(true);



                cM[i].unSelection = false;
                cM[i].timerUnSelection = 0;

              //Debug.Log(cM[i].gameObject.name);
                cM[i].GetComponent<CellScript>().isInRotationInverse = true;
                */
                cM[i].RotationButtonRight();

            }
        }
    }


    public void PlayHasEndedSound()
    {
        if (LeMouvementSeTermine != null)
            LeMouvementSeTermine.Play(0);
    }

    public void PlaySelectionSound()
    {
        if (SelectionDUnePièce != null)
            SelectionDUnePièce.Play(0);
    }

    public void PlayIsOpenSound()
    {
        if (UnePièceEstOpen != null)
            UnePièceEstOpen.Play(0);
    }

    public void PlayMovingRoomsSound()
    {
        if (DéplacementDesPièces.Count > 0)
        {
            DéplacementDesPièces[Random.Range(0, DéplacementDesPièces.Count)].Play(0);
        }
    }

    public void PlayUnSelectionSound()
    {
        if (DésélectionDUnePièce != null)
            DésélectionDUnePièce.Play(0);
    }

    public void PlayClicQuandDeuxPortesSeFontFaceSound()
    {
        if (ClicQuandDeuxPortesSeFontFace != null)
            ClicQuandDeuxPortesSeFontFace.Play(0);
    }

    public void PlayRotationDesPièces()
    {
        if (RotationDesPièces.Count > 0)
            RotationDesPièces[Random.Range(0, RotationDesPièces.Count)].Play(0);
    }

}