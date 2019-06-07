using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class OpacityKiller : MonoBehaviour
{

    public CinemachineDollyCart DollyCart;
    public List<GameObject> KillChilds;

    private float Opacity01 = 250;

    public bool debug;
    public AnimationCurve animCurve01;
    public AnimationCurve animCurve02;
    public AnimationCurve animCurve03;
    public AnimationCurve animCurve04;

    public List<SpriteRenderer> Planes;

    private int localPlaneIndex;
    private int localAlpha;
    private bool upAndRunning;

    public bool BlackRoom;
    public ScrEnvironment myEnviro;
    private bool once;
    private int timer;

    void Update()
    {
        //  Debug.Log("DesactCells       " + CellPlacement.Instance.DesactCells);
        // Debug.Log("DoOnce       " + CellPlacement.Instance.doOnce);
        //   Debug.Log("Blackroom      " + BlackRoom);


        if (!BlackRoom /*&& CellPlacement.Instance.DesactCells == false*/)
        {
            if (upAndRunning == true)
            {

                if (localAlpha == 0)
                {
                    Opacity01 = animCurve01.Evaluate(DollyCart.m_Position);
                }
                if (localAlpha == 1)
                {
                    Opacity01 = animCurve02.Evaluate(DollyCart.m_Position);
                }
                if (localAlpha == 3)
                {
                    Opacity01 = animCurve03.Evaluate(DollyCart.m_Position);
                }
                if (localAlpha == 4)
                {
                    Opacity01 = animCurve04.Evaluate(DollyCart.m_Position);
                }

                for (int i = 0; i < Planes.Count; i++)
                {
                    Planes[i].color = new Color32((byte)Planes[i].color.r, (byte)Planes[i].color.g, (byte)Planes[i].color.b, (byte)Opacity01);

                }

            }




            if (Planes[0].color.a == 1)
            {
                if (KillChilds[KillChilds.Count - 1].activeInHierarchy == true)
                {
                    for (int i = 0; i < KillChilds.Count; i++)
                    {
                        KillChilds[i].SetActive(false);

                    }
                }
            }
            else
            {
                if (KillChilds[KillChilds.Count - 1].activeInHierarchy == false)
                {
                    for (int i = 0; i < KillChilds.Count; i++)
                    {
                        KillChilds[i].SetActive(true);

                    }
                }
            }
        }

        if (BlackRoom)
        {

            for (int i = 0; i < Planes.Count; i++)
            {
                Planes[i].color = new Color32((byte)Planes[i].color.r, (byte)Planes[i].color.g, (byte)Planes[i].color.b, 255);

            }


            if (myEnviro.doorWayPoints.Count > 0)
            {
                if (myEnviro.doorWayPoints[myEnviro.doorWayPoints.Count - 1].gameObject.activeInHierarchy == true)
                {
                    for (int o = 0; o < myEnviro.doorWayPoints.Count; o++)
                    {
                        myEnviro.doorWayPoints[o].gameObject.SetActive(false);

                    }

                }
            }

            if (myEnviro.HatchesWayPoints.Count > 0)
            {
                if (myEnviro.HatchesWayPoints[myEnviro.HatchesWayPoints.Count - 1].gameObject.activeInHierarchy == true)
                {
                    for (int p = 0; p < myEnviro.HatchesWayPoints.Count; p++)
                    {
                        myEnviro.HatchesWayPoints[p].gameObject.SetActive(false);

                    }

                }

            }

            if (KillChilds.Count != 0)
            {
                if (KillChilds[KillChilds.Count - 1].activeInHierarchy == true)
                {
                    for (int r = 0; r < KillChilds.Count; r++)
                    {
                        KillChilds[r].SetActive(false);
                        GetComponent<ScrEnvironment>().doorWayPoints[0].gameObject.SetActive(false);

                    }

                }
            }

        }
        else /*(!BlackRoom && !CellPlacement.Instance.DesactCells)*/
        {

            if (myEnviro.doorWayPoints.Count > 0)
            {
                if (myEnviro.doorWayPoints[myEnviro.doorWayPoints.Count - 1].gameObject.activeInHierarchy == false)
                {
                    for (int o = 0; o < myEnviro.doorWayPoints.Count; o++)
                    {
                        myEnviro.doorWayPoints[o].gameObject.SetActive(true);

                    }

                }
            }

            if (myEnviro.HatchesWayPoints.Count > 0)
            {
                if (myEnviro.HatchesWayPoints[myEnviro.HatchesWayPoints.Count - 1].gameObject.activeInHierarchy == false)
                {
                    for (int p = 0; p < myEnviro.HatchesWayPoints.Count; p++)
                    {
                        myEnviro.HatchesWayPoints[p].gameObject.SetActive(true);

                    }

                }

            }
        }


        /*
        if (CellPlacement.Instance.DesactCells)
        {

            if (GetComponent<CellMovement>().isSpawn == false && CellPlacement.Instance.doOnce)
            {
                for (int i = 0; i < Planes.Count; i++)
                {
                    Planes[i].color = new Color32((byte)Planes[i].color.r, (byte)Planes[i].color.g, (byte)Planes[i].color.b, 255);

                }


                if (myEnviro.doorWayPoints.Count > 0)
                {
                    if (myEnviro.doorWayPoints[myEnviro.doorWayPoints.Count - 1].gameObject.activeInHierarchy == true)
                    {
                        for (int o = 0; o < myEnviro.doorWayPoints.Count; o++)
                        {
                            myEnviro.doorWayPoints[o].gameObject.SetActive(false);

                        }

                    }
                }

                if (myEnviro.HatchesWayPoints.Count > 0)
                {
                    if (myEnviro.HatchesWayPoints[myEnviro.HatchesWayPoints.Count - 1].gameObject.activeInHierarchy == true)
                    {
                        for (int p = 0; p < myEnviro.HatchesWayPoints.Count; p++)
                        {
                            myEnviro.HatchesWayPoints[p].gameObject.SetActive(false);

                        }

                    }

                }

                if (KillChilds[KillChilds.Count - 1].activeInHierarchy == true)
                {
                    for (int r = 0; r < KillChilds.Count; r++)
                    {
                        if (r < KillChilds.Count)
                        {
                            KillChilds[r].SetActive(false);
                            GetComponent<ScrEnvironment>().doorWayPoints[0].gameObject.SetActive(false);
                        }


                    }

                }


                timer++;
                if (timer == 5)
                {
                    CellPlacement.Instance.doOnce = false;
                    timer = 0;
                }

            }
            
        }*/


    }

    public void SetBlackRoomTrue()
    {
        BlackRoom = true;
    }

    public void SetBlackRoomFalse()
    {
        BlackRoom = false;
    }


    public void OpacityCheck(/*int planeIndex,*/ int alpha)
    {
        upAndRunning = true;


        //  localPlaneIndex = planeIndex;
        localAlpha = alpha;


    }




}