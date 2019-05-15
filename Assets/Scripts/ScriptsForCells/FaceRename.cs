using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceRename : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Face")
        {
            other.gameObject.name = gameObject.name;
        }
    }
}
