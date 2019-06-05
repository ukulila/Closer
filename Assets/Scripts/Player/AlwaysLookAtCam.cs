using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAtCam : MonoBehaviour
{
    public Camera cam;
    /* public float x;
     public float z;
     public float y;*/

    /* public Vector3 orientation;
     public Vector3 offset;*/


    public Animator animator;


    public void OnEnable()
    {
        animator.SetTrigger("In");
    }
    public void Start()
    {
        ///orientation = new Vector3(-cam.transform.position.x, 0, -cam.transform.position.z);
    }

    public void Update()
    {/*
        Vector3 lookTo = new Vector3(-cam.transform.position.x, 0, -cam.transform.position.z);

        transform.LookAt(lookTo);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) + offset;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(transform.localPosition);
        }*/
    }
}
