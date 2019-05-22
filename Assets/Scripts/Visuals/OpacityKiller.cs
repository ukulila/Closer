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

    void Update()
    {
        if (!BlackRoom)
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

        }



    }





    public void OpacityCheck(/*int planeIndex,*/ int alpha)
    {
        upAndRunning = true;


        //  localPlaneIndex = planeIndex;
        localAlpha = alpha;


    }




}