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

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && oui)
        {
            Debug.Log(transform.position);
        }

        //Renamer();

        if(gameObject.name.Contains("Up") == false && gameObject.name.Contains("Down") == false && isActive)
        {
            opaciteVar = 1;
        }


        distance = Vector3.Distance(transform.position, cam.transform.position);

        myMaterial.SetFloat("_Opacity", opaciteVar);

        if (!isActive)
        {
            if (distance < 2.5f)
            {
                if (opaciteVar > (-1.9f + distance) / 2)
                {
                    opaciteVar -= 0.01f;
                }
                else
                {
                    opaciteVar = (-1.9f + distance) / 2;
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

    /*private void Renamer()
    {
        if (transform.position.y == 0.5f && transform.name != "PlaneUp")
        {
            transform.name = "PlaneUp";
        }
        if (transform.position.y == -0.5f && transform.name != "PlaneDown")
        {
            transform.name = "PlaneDown";
        }
        if (transform.position.x == 0.5f && transform.name != "PlaneForward")
        {
            transform.name = "PlaneForward";
            Debug.Log("renamed  " + "   Forward");
        }
        if (transform.position.x == -0.5f && transform.name != "PlaneAway")
        {
            transform.name = "PlaneAway";
            Debug.Log("renamed  " + "   Away");
        }
        if (transform.position.z == 0.5f && transform.name != "PlaneRight")
        {
            transform.name = "PlaneRight";
            Debug.Log("renamed  " + "   Right");
        }
        if (transform.position.z == -0.5f && transform.name != "PlaneLeft")
        {
            transform.name = "PlaneLeft";
            Debug.Log("renamed  " + "   Left");
        }
    }*/
}
