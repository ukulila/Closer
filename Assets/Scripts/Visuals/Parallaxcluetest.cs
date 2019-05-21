using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxcluetest : MonoBehaviour
{
    public Vector3 mousePos;
    public Camera cam;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Displacement();



    }

    public void Displacement()
    {
        for (int i = 0; i < transform.childCount; i++)
        {

            transform.GetChild(i).position =  new Vector3 ( mousePos.x* (i-1)*5, mousePos.y * (i-1)*5, 0) /250;

        } 
            
            
            
            
    }
}
