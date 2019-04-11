using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityKiller : MonoBehaviour
{
    public Material myMaterial;
    public GameObject cam;
    public float distance;
    private float opaciteVar;
    public bool isActive;

    public bool oui;

    [Range(0.001f, 50)]
    public float distanceToFade;

    [Range(0.001f, 1)]
    public float minFade;

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        Outliner();

        if (Input.GetKeyDown(KeyCode.Space) && oui)
        {
            Debug.Log(transform.position);
        }

        //Renamer();

        if (gameObject.name.Contains("Up") == false && gameObject.name.Contains("Down") == false && isActive)
        {
            opaciteVar = 1;
        }


        distance = Vector3.Distance(transform.position, cam.transform.position);

        myMaterial.SetFloat("_Opacity", opaciteVar);

        if (!isActive)
        {
            if (distance < distanceToFade)
            {
                if (opaciteVar > minFade)
                {
                    opaciteVar -= 0.01f;
                }
                else
                {
                    opaciteVar = minFade;
                }
            }
            else
            {
                if (opaciteVar < 1)
                {
                    opaciteVar += 0.01f;
                }
                else
                {
                    opaciteVar = 1;
                }

            }
        }

        //Debug.Log(distance);
    }



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
    }

    public void Outliner()
    {
        if (/*transform.parent.name.Contains("Current") == false ||*/ transform.parent.name.Contains("Exit") == false)
        {
           /* if (transform.parent.GetComponent<CellMovement>()!=null)
            {*/
                if ((transform.parent.GetComponent<CellMovement>().selected  && myMaterial.GetInt("_isActive") != 1))
                {
                    myMaterial.SetColor("_myColor", Color.green);

                    myMaterial.SetInt("_isActive", 1);
                }
                else if (transform.parent.GetComponent<CellMovement>().selected == false && myMaterial.GetInt("_isActive") != 0)
                {

                    myMaterial.SetInt("_isActive", 0);
                }
          //  }
            /*
            if ( transform.parent.parent.parent.parent.GetComponent<CellMovement>() != null)
            {
                if ( transform.parent.parent.parent.parent.GetComponent<CellMovement>().selected && myMaterial.GetInt("_isActive") != 1)
                {
                    myMaterial.SetColor("_myColor", Color.green);

                    myMaterial.SetInt("_isActive", 1);
                }
                else if (transform.parent.parent.parent.parent.GetComponent<CellMovement>().selected == false && myMaterial.GetInt("_isActive") != 0)
                {

                    myMaterial.SetInt("_isActive", 0);
                }
            }*/




        }

        if (/*(transform.parent.name.Contains("Current") ||*/ transform.parent.name.Contains("Exit") && myMaterial.GetInt("_isActive") != 1)
        {


            myMaterial.SetInt("_isActive", 1);
        }

        if (transform.parent.GetComponent<CellMovement>().isOpen == true)
        {


            myMaterial.SetColor("_myColor", new Color32(140, 140, 140, 255));

            myMaterial.SetInt("_isActive", 1);

        }
    }





}
