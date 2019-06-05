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

    public void Awake()
    {
        doOnce = true;

        Instance = this;
        Application.targetFrameRate = 400;

    }

    void Update()
    {

        if (GameManager.Instance.currentGameMode == GameManager.GameMode.Dialogue)
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

                    if(cM[i].selected)
                    {
                        cM[i].unSelection = true;
                        cM[i].isOpen = true;
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
            {
                cM[i].OutlineOff = false;

                cM[i].selected = false;
                cM[i].Outline.gameObject.SetActive(true);
                cM[i].CanvasRotation.gameObject.SetActive(true);

                cM[i].unSelection = false;
                cM[i].timerUnSelection = 0;

                //Debug.Log(cM[i].gameObject.name);
                cM[i].GetComponent<CellScript>().isInRotation = true;
            }
        }

    }

    public void turnRight()
    {

        for (int i = 0; i < cM.Count; i++)
        {
            if (cM[i].selected && cM[i].isSpawn == false && cM[i].GetComponent<CellScript>().isInRotation == false)
            {
                cM[i].OutlineOff = false;

                cM[i].selected = false;
                cM[i].Outline.gameObject.SetActive(true);
                cM[i].CanvasRotation.gameObject.SetActive(true);



                cM[i].unSelection = false;
                cM[i].timerUnSelection = 0;

              //Debug.Log(cM[i].gameObject.name);
                cM[i].GetComponent<CellScript>().isInRotationInverse = true;
            }
        }
    }

}
