﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{

    public CellMovement myCellMovement;
    public Material myMaterial;


    public void Start()
    {
        if (myCellMovement == null)
        {
            myCellMovement = GetComponent<CellMovement>();
        }

        if (myMaterial == null)
        {
            myMaterial = GetComponent<Renderer>().material;
        }
    }

    public void Update()
    {
        Outliner();
    }

    public void Outliner()
    {

        if (myCellMovement.selected)
        {
            myMaterial.SetColor("_myColor", Color.green);
            myMaterial.SetInt("_isActive", 1);
        }

        if (myCellMovement.selected == false)
        {

            myMaterial.SetInt("_isActive", 0);
        }

        if (myCellMovement.isOpen == true)
        {
            myMaterial.SetColor("_myColor", Color.white);
            myMaterial.SetInt("_isActive", 1);
        }


        if (myCellMovement.isSpawn)
        {
            myMaterial.SetColor("_myColor", Color.green);
            myMaterial.SetInt("_isActive", 1);
        }
    }
}
