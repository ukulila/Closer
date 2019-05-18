using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class OpacityKiller : MonoBehaviour
{
/*
    private List<Material> myMaterial;
    public GameObject cam;
    public float distance;
    private float opaciteVar;
    public bool isActive;

    public bool oui;

    [Range(0.001f, 500)]
    public float distanceToFade;

    [Range(0.001f, 1)]
    public int minFade;
    public bool tuto;
    [Range(0.001f, 1)]
    public int maxFade;

    public CellMovement myCellMovement;
    private List<GameObject> KillChilds;
    */

    public CinemachineDollyCart DollyCart;

    private float Opacity01 = 250;
    /*
    private float Opacity02;
    private float Opacity03;
    private float Opacity04;
    */
    public bool debug;
    public AnimationCurve animCurve01;
    public AnimationCurve animCurve02;
    public AnimationCurve animCurve03;
    public AnimationCurve animCurve04;

   // public int opacityState;

    public List<SpriteRenderer> Planes;

    private int localPlaneIndex;
    private int localAlpha;
    private bool upAndRunning;



    // Start is called before the first frame update
    void Start()
    {
        //  OpacitySetup();


    }
    
   /* private void OpacitySetup()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Plane") && transform.GetChild(i).name.Contains("Porte") == false)                          ///////////
            {
                myMaterial.Add(transform.GetChild(i).GetComponent<Renderer>().material);
            }
        }

        if (myCellMovement == null)                                                                                                              ///////////
        {
            myCellMovement = GetComponent<CellMovement>();
        }

        for (int i = 0; i < transform.childCount; i++)                                                                                           ///////////                                                                                            
        {

            if (transform.GetChild(i).name.Contains("Plane") == false)
            {
                KillChilds.Add(transform.GetChild(i).gameObject);
            }

        }

        if(cam == null)
        {
            cam = GameObject.Find("Main Camera");
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        
          if(upAndRunning == true)
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
          /*
                float delay = 1f;

                if (Time.time % delay < Time.deltaTime)
                {*/
        //distance = Vector3.Distance(transform.position, cam.transform.position);




        /*
        for (int i = 0; i < myMaterial.Count; i++)
        {

            myMaterial[i].SetFloat("_Opacity", opaciteVar);

        }*/
    }

    // Opacity01 = animCurve01.Evaluate(DollyCart.m_Position);



    /*
    Cache[0].color = new Color32((byte)Cache[0].color.r, (byte)Cache[0].color.g, (byte)Cache[0].color.b, (byte)Opacity01);
    Cache[1].color = new Color32((byte)Cache[1].color.r, (byte)Cache[1].color.g, (byte)Cache[1].color.b, (byte)Opacity01);
    Cache[2].color = new Color32((byte)Cache[2].color.r, (byte)Cache[2].color.g, (byte)Cache[2].color.b, (byte)Opacity01);

    Opacity02 = animCurve02.Evaluate(DollyCart.m_Position);

    Cache[3].color = new Color32((byte)Cache[3].color.r, (byte)Cache[3].color.g, (byte)Cache[3].color.b, (byte)Opacity02);
    Cache[4].color = new Color32((byte)Cache[4].color.r, (byte)Cache[4].color.g, (byte)Cache[4].color.b, (byte)Opacity02);
    Cache[5].color = new Color32((byte)Cache[5].color.r, (byte)Cache[5].color.g, (byte)Cache[5].color.b, (byte)Opacity02);


    Opacity03 = animCurve03.Evaluate(DollyCart.m_Position);

    Cache[6].color = new Color32((byte)Cache[6].color.r, (byte)Cache[6].color.g, (byte)Cache[6].color.b, (byte)Opacity03);
    Cache[7].color = new Color32((byte)Cache[7].color.r, (byte)Cache[7].color.g, (byte)Cache[7].color.b, (byte)Opacity03);
    Cache[8].color = new Color32((byte)Cache[8].color.r, (byte)Cache[8].color.g, (byte)Cache[8].color.b, (byte)Opacity03);


    Opacity04 = animCurve04.Evaluate(DollyCart.m_Position);

    Cache[9].color = new Color32((byte)Cache[9].color.r, (byte)Cache[9].color.g, (byte)Cache[9].color.b, (byte)Opacity04);
    Cache[10].color = new Color32((byte)Cache[10].color.r, (byte)Cache[10].color.g, (byte)Cache[10].color.b, (byte)Opacity04);
    Cache[11].color = new Color32((byte)Cache[11].color.r, (byte)Cache[11].color.g, (byte)Cache[11].color.b, (byte)Opacity04);
    */




    //Outliner();
    //Disapearance();

    /* Cache[2].color = new Vector4(Cache[0].color.r, Cache[0].color.g, Cache[0].color.b, Opacity02);
     Cache[3].color = new Vector4(Cache[0].color.r, Cache[0].color.g, Cache[0].color.b, Opacity02);
     Cache[4].color = new Vector4(Cache[1].color.r, Cache[1].color.g, Cache[1].color.b, Opacity03);
     Cache[5].color = new Vector4(Cache[1].color.r, Cache[1].color.g, Cache[1].color.b, Opacity03);
     Cache[6].color = new Vector4(Cache[1].color.r, Cache[1].color.g, Cache[1].color.b, Opacity04);
     Cache[7].color = new Vector4(Cache[1].color.r, Cache[1].color.g, Cache[1].color.b, Opacity04);*/




    /// Is FPS ok, but opacity
    /// 
    /* if (!isActive)
     {

    if (debug)
    {
        Debug.Log(Opacity01);
    }

    if (DollyCart.m_Position <= 156 && DollyCart.m_Position >= 95 || DollyCart.m_Position < 20)
    {

        Debug.Log("Path" + DollyCart.m_Path.PathLength);
        // if (Opacity01 > minFade)
        //   {
        if (Opacity01 > 5)
        {
        }

        if (Opacity01 < 5)
        {
            Opacity01 = 0;
        }

        Debug.Log("Between 140 and 101");
        //Opacity01 = 0;
        // }
        else
        {
            Opacity01 = maxFade;
        }
    }
    else
    {
         if (Opacity01 < maxFade)
          {
        Debug.Log("NOT Between 140 and 101");
        //Opacity01 = animCurveIncrease.Evaluate(Time.deltaTime);

        //Opacity01 = 250;
        // }
        else
        {
            Opacity01 = minFade;
        }

    }
}
*/

    public void OpacityCheck(/*int planeIndex,*/ int alpha)
    {
        upAndRunning = true;


      //  localPlaneIndex = planeIndex;
        localAlpha = alpha;

 
    }







    /*

    /// <summary>
    ///  FPS Ok
    /// </summary>ye
    public void Disapearance()
    {
        if (opaciteVar == maxFade)
        {
            for (int i = 0; i < KillChilds.Count; i++)
            {

                KillChilds[i].SetActive(false);

            }
        }
        else
        {
            for (int i = 0; i < KillChilds.Count; i++)
            {

                KillChilds[i].SetActive(true);

            }
        }


    }

    /// <summary>
    /// This is FPS ok
    /// </summary>
    public void Outliner()
    {
        if (myCellMovement != null)
        {

            if (myCellMovement.selected)
            {
                for (int i = 0; i < myMaterial.Count; i++)
                {
                    myMaterial[i].SetColor("_myColor", Color.green);
                    myMaterial[i].SetInt("_isActive", 1);
                }
            }

            if (myCellMovement.selected == false)
            {
                for (int i = 0; i < myMaterial.Count; i++)
                {
                    myMaterial[i].SetInt("_isActive", 0);
                }
            }

            if (myCellMovement.isOpen == true)
            {
                for (int i = 0; i < myMaterial.Count; i++)
                {
                    myMaterial[i].SetColor("_myColor", Color.white);
                    myMaterial[i].SetInt("_isActive", 1);
                }
            }


        }


        if (tuto)
        {
            for (int i = 0; i < myMaterial.Count; i++)
            {
                myMaterial[i].SetColor("_myColor", Color.green);

                myMaterial[i].SetInt("_isActive", 1);
            }
        }
    }*/


}