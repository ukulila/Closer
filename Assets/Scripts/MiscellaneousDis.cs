using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousDis : MonoBehaviour
{
    public MeshRenderer mR;

    public void OnTriggerEnter(Collider other)
    {
       /* if (other.gameObject.name == "PlaneLeft" || other.gameObject.name == "PlaneRight" || other.gameObject.name == "PlaneAway" || other.gameObject.name == "PlaneForward")
        {
            mR = GetComponent<MeshRenderer>();
            mR.enabled = false;
        }*/
        if (other.gameObject.name == "BlankPlane")
        {
            mR = GetComponent<MeshRenderer>();
            mR.enabled = true;
        }

        if (other.gameObject.name == "PlaneLeft" || other.gameObject.name == "PlaneRight" || other.gameObject.name == "PlaneAway" || other.gameObject.name == "PlaneForward")
        {
            mR = GetComponent<MeshRenderer>();
            mR.enabled = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        /*
        if (other.gameObject.name == "BlankPlane")
        {
            mR = GetComponent<MeshRenderer>();
            mR.enabled = true;
        }*/

    }
}
