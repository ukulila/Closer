using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityKiller : MonoBehaviour
{

    public List<Material> myMaterial;
    public GameObject cam;
    public float distance;
    private float opaciteVar;
    public bool isActive;

    public bool oui;

    [Range(0.001f, 500)]
    public float distanceToFade;

    [Range(0.001f, 1)]
    public float minFade;
    public bool tuto;
    [Range(0.001f, 1)]
    public float maxFade;

    public CellMovement myCellMovement;
    public List<GameObject> KillChilds;



    public void OpacitySetup()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Plane") && transform.GetChild(i).name.Contains("Porte") == false)                          ///////////
            {
                myMaterial.Add(transform.GetChild(i).GetComponent<Renderer>().sharedMaterial);
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
    }

    // Update is called once per frame
    void Update()
    {
        float delay = 1f;

        if (Time.time % delay < Time.deltaTime)
        {
            distance = Vector3.Distance(transform.position, cam.transform.position);

            for (int i = 0; i < myMaterial.Count; i++)
            {

                myMaterial[i].SetFloat("_Opacity", opaciteVar);

            }
        }


        Outliner();
        Disapearance();


        

        /// Is FPS ok, but opacity
        /// 
        if (!isActive)
        {

            if (distance < distanceToFade)
            {
                if (opaciteVar > 0)
                {
                    opaciteVar -= 0.01f;
                }
                else
                {
                    opaciteVar = 0;
                }
            }
            else
            {
                if (opaciteVar < maxFade)
                {
                    opaciteVar += 0.01f;
                }
                else
                {
                    opaciteVar = maxFade;
                }

            }
        }

    }


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
    }


}