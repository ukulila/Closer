﻿using System.Collections;
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
    public bool okToSetup;
    public bool isInRotation;

    public GameObject facingPlane;



    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
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


                    if (Cui.switchToUI == true)
                    {
                        cellmove.hasEnded = false;
                    }

                    if (cellmove.isSpawn == true)
                    {
                        for (int i = 0; i < cM.Count; i++)
                        {
                            cM[i].isOpen = false;
                        }
                        cellmove.raycastAutor = true;
                    }




                    if (cellmove.isOpen)
                    {
                        cellmove.clickDirection = true;
                    }

                    cellmove.originPos = Input.mousePosition;

                    if (cB != null)
                        cB.aboutCamera = false;

                    if (Cr != null)
                        Cr.aboutCamera = false;
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

                }
            }
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

}
