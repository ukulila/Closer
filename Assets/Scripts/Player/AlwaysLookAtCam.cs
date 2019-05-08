using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAtCam : MonoBehaviour
{
    public Camera cam;
    public float x;
    public float z;
    public float y;

    public void Update()
    {
        Vector3 lookTo = new Vector3(-cam.transform.position.x + x,90 + y, -cam.transform.position.z+z);
        transform.LookAt(lookTo);

        //transform.localPosition.x
        //transform.localPosition.z
        //transform.localPosition.y
    }
}
