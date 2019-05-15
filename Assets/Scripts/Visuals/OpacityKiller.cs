using System;
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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Plane") && transform.GetChild(i).name.Contains("Porte") == false)
            {
                myMaterial.Add(transform.GetChild(i).GetComponent<Renderer>().material);
            }
        }
        StartCoroutine(CalculateDistance());

        if (myCellMovement == null)
        {
            myCellMovement = GetComponent<CellMovement>();
        }

    }

    // Update is called once per frame
    void Update()
    {

        Outliner();
        Disapearance();

        //Renamer();
       /* if (gameObject.name.Contains("Up") == false && gameObject.name.Contains("Down") == false && isActive)
        {
            opaciteVar = 1;
        }
        */
        for (int i = 0; i < myMaterial.Count; i++)
        {
            myMaterial[i].SetFloat("_Opacity", opaciteVar);

        }

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

    public void Disapearance()
    {
        if (opaciteVar == maxFade)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("Plane") == false)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("Plane") == false)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

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


    WaitForSeconds wait = new WaitForSeconds(1f);


    public IEnumerator CalculateDistance()
    {
        while (true)
        {
            distance = Vector3.Distance(transform.position, cam.transform.position);
            yield return wait;
        }

    }



}
