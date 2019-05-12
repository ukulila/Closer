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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Plane"))
            {
                myMaterial.Add(transform.GetChild(i).GetComponent<Renderer>().material);
            }
        }
        StartCoroutine("CalculateDistance");

    }

    // Update is called once per frame
    void Update()
    {

        Outliner();

        //Renamer();
        if (gameObject.name.Contains("Up") == false && gameObject.name.Contains("Down") == false && isActive)
        {
            opaciteVar = 1;
        }

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

        //Debug.Log(distance);
    }
    /*
    public void OnTriggerStay(Collider other)
    {

        if (other.name == "OpacityToZero")
        {
            isActive = true;
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "OpacityToZero")
        {
            isActive = false;
        }
    }*/

    public void Outliner()
    {
        if (GetComponent<CellMovement>() != null)
        {
            if (transform.name.Contains("Exit") == false)
            {
                for (int i = 0; i < myMaterial.Count; i++)
                {
                    if ((GetComponent<CellMovement>().selected && myMaterial[i].GetInt("_isActive") != 1))
                    {

                        myMaterial[i].SetColor("_myColor", Color.green);

                        myMaterial[i].SetInt("_isActive", 1);
                    }
                    else if (GetComponent<CellMovement>().selected == false && myMaterial[i].GetInt("_isActive") != 0)
                    {

                        myMaterial[i].SetInt("_isActive", 0);
                    }

                }
            }

            for (int i = 0; i < myMaterial.Count; i++)
            {
                if (transform.name.Contains("Exit") && myMaterial[i].GetInt("_isActive") != 1)
                {


                    myMaterial[i].SetInt("_isActive", 1);
                }
            }

            for (int i = 0; i < myMaterial.Count; i++)
            {
                if (transform.GetComponent<CellMovement>().isOpen == true)
                {


                    myMaterial[i].SetColor("_myColor", new Color32(140, 140, 140, 255));

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

    public IEnumerator CalculateDistance()
    {
        while (true)
        {
            distance = Vector3.Distance(transform.position, cam.transform.position);
            yield return new WaitForSeconds(1.5f);
        }

    }



}
