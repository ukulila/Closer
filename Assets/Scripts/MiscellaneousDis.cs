using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousDis : MonoBehaviour
{
    public MeshRenderer mR;

    public void Start()
    {
        mR = GetComponent<MeshRenderer>();
    }
    public void OnTriggerStay(Collider other)
    {
        /* if (other.gameObject.name == "PlaneLeft" || other.gameObject.name == "PlaneRight" || other.gameObject.name == "PlaneAway" || other.gameObject.name == "PlaneForward")
         {
             mR = GetComponent<MeshRenderer>();
             mR.enabled = false;
         }*/
        if (other.gameObject.GetComponent<OpacityKiller>() != null)
        {
            if (other.gameObject.GetComponent<OpacityKiller>().isActive == false)
            {

                mR.enabled = false;

                if (transform.childCount != 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }

        if (other.gameObject.GetComponent<OpacityKiller>() != null)
        {
            if (other.gameObject.GetComponent<OpacityKiller>().isActive == true)
            {
                mR.enabled = true;
                if (transform.childCount != 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    /* public void OnTriggerExit(Collider other)
     {

         if (other.gameObject.name == "BlankPlane")
         {
             mR = GetComponent<MeshRenderer>();
             mR.enabled = true;
         }

     }*/
}
