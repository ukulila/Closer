using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousDis : MonoBehaviour
{
    public MeshRenderer mR;
    public bool invisibleOnSpawn;

    public void Start()
    {
        mR = GetComponent<MeshRenderer>();

        if(invisibleOnSpawn)
        {
            mR.enabled = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        /* if (other.gameObject.name == "PlaneLeft" || other.gameObject.name == "PlaneRight" || other.gameObject.name == "PlaneAway" || other.gameObject.name == "PlaneForward")
         {
             mR = GetComponent<MeshRenderer>();
             mR.enabled = false;
         }*/


        if (other.name == "OpacityToZero")
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



        /*
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

    }
}*/
    }

    public void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "OpacityToZero")
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
}
